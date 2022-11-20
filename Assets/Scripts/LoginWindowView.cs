using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;


public class LoginWindowView : MonoBehaviour
{
    public static LoginWindowView instance1;
    // Debug Flag to simulate a reset
    public bool ClearPlayerPrefs;

    // Meta fields for objects in the UI
    public InputField Username;
    public InputField Password;
    public InputField ConfirmPassword;
    public InputField ScoreName;
    public Toggle RememberMe;

    public Button LoginButton;
    public Button RegisterButton;
    public Button CancelRegisterButton;
    public Button ForgotYourPassword;

    // Meta references to panels we need to show / hide
    public GameObject FirstPanel;
    public GameObject RegisterPanel;
    public GameObject SigninPanel;
    public GameObject Panel;
    public GameObject ConnectionErrorPanel;
    public Text StatusText;

    // Settings for what data to get from playfab on login.
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;


    // Reference to our Authentication service
    private PlayFabAuthService _AuthService = PlayFabAuthService.Instance;
    protected static LoginWindowView instance = null;

    public void Awake()
    {
        if (ClearPlayerPrefs)
        {
            _AuthService.UnlinkSilentAuth();
            _AuthService.ClearRememberMe();
            _AuthService.AuthType = Authtypes.None;
        }

        //Set our remember me button to our remembered state.
        RememberMe.isOn = _AuthService.RememberMe;

        //Subscribe to our Remember Me toggle
        RememberMe.onValueChanged.AddListener(
            (toggle) =>
            {
                _AuthService.RememberMe = toggle;
            });
    }

    public void Start()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);


            instance = this;

        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);

            }

            
        }

        // Hide all our panels until we know what UI to display
        Panel.SetActive(false);
        RegisterPanel.SetActive(false);
        SigninPanel.SetActive(false);
        FirstPanel.SetActive(true);
        

        // Subscribe to events that happen after we authenticate
        PlayFabAuthService.OnDisplayAuthentication += OnDisplayAuthentication;
        PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += OnPlayFaberror;

        // Bind to UI buttons to perform actions when user interacts with the UI.
        LoginButton.onClick.AddListener(OnLoginClicked);
        //PlayAsGuestButton.onClick.AddListener(OnPlayAsGuestClicked);
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);
        CancelRegisterButton.onClick.AddListener(OnCancelRegisterButtonClicked);

        // Set the data we want at login from what we chose in our meta data.
        _AuthService.InfoRequestParams = InfoRequestParams;
        // Start the authentication process.
        _AuthService.Authenticate();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            FirstPanel.SetActive(false);
            ConnectionErrorPanel.SetActive(true);
            return;
        }
    }



    /// <summary>
    /// Login Successfully - Goes to next screen.
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        PlayerPrefs.SetString("Email", Username.text);
        PlayerPrefs.SetString("PlayFabId", result.PlayFabId);
        Debug.LogFormat("Logged In as: {0}", result.PlayFabId);
        Username.text = string.Empty;
        Password.text = string.Empty;
        ConfirmPassword.text = string.Empty;
        ScoreName.text = string.Empty;
        StatusText.text = string.Empty;
        RegisterPanel.SetActive(false);
        Panel.SetActive(false);

        PlayFabManager.IsLoggedIn = true;

        
        //PlayFabManager.LoadUserData();
        //PlayFabManager.LoadAccountData();
        //PlayFabManager.LoadTitleData();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Error handling for when Login returns errors.
    /// </summary>
    /// <param name="error"></param>
    private void OnPlayFaberror(PlayFabError error) //PlayFabError
    {

        switch (error.Error)
        {   
            case PlayFabErrorCode.InvalidParams:
                if (Password.text.Length < 6)
                    StatusText.text = "Password must be at least 6 characters!";
                else
                    StatusText.text = "Invalid Email or password!";
                break;
            
            case PlayFabErrorCode.AccountNotFound:
                StatusText.text = "Account Not Found, create an account now.";
                RegisterPanel.SetActive(true);
                SigninPanel.SetActive(false);
                break;
            case PlayFabErrorCode.AccountBanned:
                StatusText.text = "Account Banned";
                break;
            default:
                StatusText.text = "Invalid password!";
                break;

                // default else
                //  StatusText.text = string.Format("Error {0}: {1}", error.HttpCode, error.ErrorMessage);
                //if (OnLoginFail != null)
                //OnLoginFail(errorMessage, MessageDisplayStyle.error);
                // reset these IDs (a hack for properly detecting if a device is claimed or not, we will have an API call for this soon)
                //PlayFabLoginCalls.android_id = string.Empty;
                //PlayFabLoginCalls.ios_id = string.Empty;
                //clear the token if we had a fb login fail
        }

    }



    /// <summary>
    /// Choose to display the Auth UI or any other action.
    /// </summary>
    private void OnDisplayAuthentication()
    {
        //Here we have choses what to do when AuthType is None.
        Panel.SetActive(true);
        if (PlayerPrefs.GetFloat("ComeBack") == 1f){
            SigninPanel.SetActive(true);
            PlayerPrefs.SetFloat("ComeBack", 0f);
        }
    }

    /// <summary>
    /// Play As a guest, which means they are going to silently authenticate
    /// by device ID or Custom ID
    /// </summary>
    /*private void OnPlayAsGuestClicked()
    {

        StatusText.text = "Logging In As Guest ...";

        _AuthService.Authenticate(Authtypes.Silent);
    }*/

    /// <summary>
    /// Login Button means they've selected to submit a username (email) / password combo
    /// Note: in this flow if no account is found, it will ask them to register.
    /// </summary>
    private void OnLoginClicked()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
           
            ConnectionErrorPanel.SetActive(true);
            
        }

        //LogOut();


        StatusText.text = string.Format("Logging In As {0}", Username.text);

        _AuthService.Email = Username.text;
        _AuthService.Password = Password.text;
        _AuthService.Authenticate(Authtypes.EmailAndPassword);
    }

    /// <summary>
    /// No account was found, and they have selected to register a username (email) / password combo.
    /// </summary>
    private void OnRegisterButtonClicked()
    {
        if (Password.text != ConfirmPassword.text)
        {
            StatusText.text = "Passwords do not Match.";
            return;
        }

        PlayerPrefs.SetFloat("Registrazione", 1f);

        StatusText.text = string.Format("Registering User {0}", Username.text);
        PlayerPrefs.SetString("TitleDisplayName", ScoreName.text);
        _AuthService.Email = Username.text;
        _AuthService.Password = Password.text;
        _AuthService.ScoreName = ScoreName.text;
        _AuthService.Authenticate(Authtypes.RegisterPlayFabAccount);

    }


    /// <summary>
    /// They have opted to cancel the Registration process.
    /// Possibly they typed the email address incorrectly.
    /// </summary>
    private void OnCancelRegisterButtonClicked()
    {
        // Reset all forms
        Username.text = string.Empty;
        Password.text = string.Empty;
        ConfirmPassword.text = string.Empty;
        ScoreName.text = string.Empty;
        StatusText.text = string.Empty;

        // Show panels
        RegisterPanel.SetActive(false);
        SigninPanel.SetActive(true);
    }


    /*Funzione che permette di modificare la password se sbagliata utilizzando il funzione RecoveryEmailRequest*/
    public void ResetPassword()
    {
        string text = Username.text;
        if (text != "")
        {
            var request = new SendAccountRecoveryEmailRequest();
            request.Email = text;
            request.TitleId = "46D5A";
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoveryEmailSuccess, OnPlayFaberror);
        }
    }

    public void OnRecoveryEmailSuccess(SendAccountRecoveryEmailResult obj)
    {
        UnityEngine.Debug.Log("E-mail succesfully send!");
        StatusText.text = "E-mail sent to:" + Username.text;
    }

    public void PlayOffline()
    {
        SceneManager.LoadScene("MenuOffline");
    }

    public void LogOut()
    {
        _AuthService.UnlinkSilentAuth();
        _AuthService.ClearRememberMe();
        _AuthService.AuthType = Authtypes.None;
        PlayFabClientAPI.ForgetAllCredentials();
        PlayFabManager.IsLoggedIn = false;

    }


    public void Update()
    {
        PlayerPrefs.SetString("Email", Username.text);
    }

    void OnApplicationQuit()
    {
        if (!RememberMe.isOn)
        {
            LogOut();
            Debug.Log("Log out fatto!");
        }
        
    }

    public void DeactivateText()
    {
        StatusText.text = string.Empty;
    }



}