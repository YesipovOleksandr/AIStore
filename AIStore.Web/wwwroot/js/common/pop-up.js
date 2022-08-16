
function CloseLoginModal() {
let popup = document.getElementById('popupLogin');
  popup.style.display = "none";
}

function ShowLoginModal() {
    let popup = document.getElementById('popupLogin');
    popup.style.display = "block";
}

function CloseSingUp() {
    let popup = document.getElementById('popupSignUp');
    popup.style.display = "none";
}

function ShowSingUp() {
    let popup = document.getElementById('popupSignUp');
    popup.style.display = "block";
}
function varificationFormLogin() {

    let email = document.querySelector('.emailPopUp');
    let password = document.getElementsByClassName('passwordPopUp')[0];

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
        alert('невозможно.');
    
    }
   
    if (regexPassword.test(password.value)) {
        password.classList.remove("error");
        errorPassword.style.display = "none";
    }
    else {
        password.classList.add("error");
        errorPassword.style.display = "block";
    }
   
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

function onInputValidationSignup() {
    varificationSignUp();
}

function varificationSignUp() {
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
    }

}
