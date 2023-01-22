if (document.readyState !== 'loading') {
    initGoogleOneTap();
} else {
    document.addEventListener('DOMContentLoaded', function () {
        initGoogleOneTap();
    });
}

function initGoogleOneTap() {

    var googleOneTapElement = document.getElementById('g_id_onload');
    googleOneTapElement.setAttribute('data-client_id', '228677941491-tohssc1v3gkflvilmpph992k6f3neijr.apps.googleusercontent.com');

    window['OnLoginCallback'] = OnLoginCallback;

    if (!isAuthorized()) {
        scriptService.loadScript("google-one-tap");
    }
}

function OnLoginCallback(responce) {
    try {
        signInGoogleToken(responce.credential).then(responce => {
            responce.json().then(authResponce => {
                AddCookieAuth(authResponce);
            })
        })
    } catch (e) {
    }
}

function signInGoogleToken(token) {

    const headers = { Authorization: token, 'Content-Type': 'application/json' };
    let resource = `api/Account/external/google-token`;

    return fetch(window.clientConfig.environmentconfig.apiurl + resource, { method: "POST", headers: headers });
}

function setAuthResponseData(resp) {
    if (resp && resp.access_token) {
        var cookieValue = encodeURIComponent(JSON.stringify(resp));
        cookieService.setCookie('auth_user', cookieValue, 1);
    }
}
