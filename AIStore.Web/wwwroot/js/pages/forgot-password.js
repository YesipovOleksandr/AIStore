function clickForgotPassword() {
    var login = document.getElementById("login");
    fetch(window.clientConfig.environmentconfig.apiurl + "api/Account/forgot-password" + "?login=" + login.value, {
        method: "GET",
    }).then(response => response.json())
}