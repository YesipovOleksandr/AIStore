var _authCookieName = "auth_user";

function externalAuth(providerValue) {
    var externalAuthUrl = window.clientConfig.environmentconfig.apiurl + `api/Account/external/login?provider=${providerValue}`;
    location.href = externalAuthUrl;
}

function isAuthorized() {
    return getCookie(_authCookieName) != null;
};

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

function AddCookieAuth(response) {
    var userResponse = {
        access_token: response.token,
        refresh_token: response.refreshToken,
        expires: response.refreshTokenExpiryTime,
    };
    document.cookie = encodeURIComponent(_authCookieName) + '=' + encodeURIComponent(JSON.stringify(userResponse));
    location.reload();
}

function Logout() {
    document.cookie = _authCookieName + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    location.reload();
}

function sendFormLogin() {
    showSpinner();
    let isvalidate = varificationFormLogin();
    if (isvalidate == false) {
        hideSpinner();
        return;
    }
    let login = document.querySelector('.emailPopUp');
    let password = document.querySelector('.passwordPopUp');
    let loginVal = login.value;
    let passwordVal = password.value;

    fetch(window.clientConfig.environmentconfig.apiurl + 'api/Account/authenticate', {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            login: loginVal,
            password: passwordVal
        })
    }).then(response => response.json())
        .then((response) => {
            if (response.status == 401) {
                console.log(response.title);
                hideSpinner();
                return;
            }
            AddCookieAuth(response);
        });
}

function varificationFormLogin() {
    let IsValidation = true;
    let email = document.querySelector('.emailPopUp');
    let password = document.querySelector('.passwordPopUp');

    let regexEmail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    let regexPassword = /(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g;
    let error = document.querySelector('.error_email');
    let errorPassword = document.querySelector('.error_password');
    if (regexEmail.test(email.value)) {
        email.classList.remove("error");
        error.style.display = "none";
    }
    else {
        email.classList.add("error");
        error.style.display = "block";
        IsValidation = false;
    }

    if (regexPassword.test(password.value)) {
        password.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        password.classList.add("error");
        errorPassword.style.display = "block";
        IsValidation = false;
    }
    return IsValidation;
}

function OnInputValidationLogin() {
    let email = document.querySelector('.emailPopUp');
    let regexEmail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    let error = document.querySelector('.error_email');
    if (regexEmail.test(email.value)) {
        email.classList.remove("error");
        error.style.display = "none";
    }
    else {
        email.classList.add("error");
        error.style.display = "block";
    }
}

function OnInputValidationLoginPassword() {
    let password = document.getElementsByClassName('passwordPopUp')[0];
    let regexPassword = /(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g;
    let errorPassword = document.querySelector('.error_password');

    if (regexPassword.test(password.value)) {
        password.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        password.classList.add("error");
        errorPassword.style.display = "block";
    }
}

function varificationSignUp() {
    let iSvalidation = true;
    let email = document.querySelector('.email_sign_up');
    let regexEmail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    let error = document.querySelector('.errorEmail');
    if (regexEmail.test(email.value)) {
        email.classList.remove("error");
        error.style.display = "none";
    }
    else {
        email.classList.add("error");
        error.style.display = "block";
        iSvalidation = false;
    }
    return iSvalidation;
}

function onInputValidationSignup() {
    varificationSignUp();
}

function SendFormIsUser() {
    showSpinner();
    let isValidate = varificationSignUp();
    if (isValidate == false) {
        hideSpinner();
        return;
    }
    let loginVal = document.querySelector('.email_sign_up').value;
    fetch(window.clientConfig.environmentconfig.apiurl + "api/Account/login-exist", {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            login: loginVal,
        })
    }).then((res) => res.json())
        .then((response) => {
            if (response.status == 200) {
                hideSpinner();
                movingOnSignUp();
            }
            else {
                let textError = response.errors['LoginError'][0];
                let serverError = document.querySelector('.serverError');
                serverError.innerHTML = textError;
                hideSpinner();
            }
        });
}

function movingOnSignUp() {
    let lastSingUp = document.querySelector('.block_left_popup_signUp');
    let nextSingUp = document.querySelector('.left_signUp_registration');
    lastSingUp.style.display = "none";
    nextSingUp.style.display = "block";
}

function defaultSignUpPage() {
    let lastSingUp = document.querySelector('.block_left_popup_signUp');
    let nextSingUp = document.querySelector('.left_signUp_registration');
    lastSingUp.style.display = "block";
    nextSingUp.style.display = "none";
}

function Registration() {
    showSpinner();
    let password = document.querySelector('.passwordPopUp_registration');
    let RepeatPassword = document.querySelector('.repeat_passwordPopUp');
    let regexPassword = /(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g;
    let errorPassword = document.querySelector('.error_signup_password');

    if (regexPassword.test(password.value) && password.value == RepeatPassword.value) {
        password.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        password.classList.add("error");
        errorPassword.style.display = "block";
        hideSpinner();
        return;
    }

    let passwordValue = password.value;
    let repeatPasswordValue = RepeatPassword.value;
    let emailValue = document.querySelector('.email_sign_up').value;

    fetch(window.clientConfig.environmentconfig.apiurl + "api/Account/registration", {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            login: emailValue,
            password: passwordValue,
            confirmPassword: repeatPasswordValue
        })
    }).then(response => response.json())
        .then((response) => {

            if (response.status == 401 || response.status == 400) {
                if (response.errors !== undefined) {
                    console.log(response.errors['RegisterError'][0]);
                } else {
                    console.log(response.title);
                }
                hideSpinner();
                return;
            } else {
                var userResponse = {
                    access_token: response.token,
                    refresh_token: response.refreshToken,
                    expires: response.refreshTokenExpiryTime,
                };
                document.cookie = encodeURIComponent(_authCookieName) + '=' + encodeURIComponent(JSON.stringify(userResponse));
                location.reload();
                CloseSingUp();
                defaultSignUpPage();
            }
        });
}

function PasswordRegistrationOnInput() {
    let password = document.querySelector('.passwordPopUp_registration');
    let regexPassword = /(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g;
    let errorPassword = document.querySelector('.error_signup_password');

    if (regexPassword.test(password.value)) {
        password.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        password.classList.add("error");
        errorPassword.style.display = "block";
    }
}

function RepeatPasswordRegistrationOnInput() {
    let RepeatPasswordValue = document.querySelector('.repeat_passwordPopUp');
    let regexPassword = /(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g;
    let errorPassword = document.querySelector('.error_password_repeat');

    if (regexPassword.test(RepeatPasswordValue.value)) {
        RepeatPasswordValue.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        RepeatPasswordValue.classList.add("error");
        errorPassword.style.display = "block";
    }
}


function OnClickSentActiveEmail() {
    var cookie = decodeURIComponent(getCookie(_authCookieName));
    var token = JSON.parse(cookie).access_token;
    fetch(window.clientConfig.environmentconfig.apiurl + "api/Account/send-activation-email", {
        method: "GET",
        headers: { 'Authorization': 'Bearer ' + token, 'Content-Type': 'application/json' }
    }).then((response) => {
        console.log(response);
    })
}

