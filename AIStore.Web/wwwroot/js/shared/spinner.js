
if (document.readyState !== 'loading') {
    addBlockSpinner();
} else {
    document.addEventListener('DOMContentLoaded', function () {
        addBlockSpinner();
    });
}

//spinner
function addBlockSpinner() {
    var spinnerContainer = document.createElement('div');
    spinnerContainer.classList.add('spinner-border');
    spinnerContainer.style.display = "none";
    var spinner = document.createElement('div');
    spinner.classList.add('spinner');
    spinnerContainer.appendChild(spinner);
    hideSpinner();
    document.body.appendChild(spinnerContainer);
}

function showSpinner() {
    let spinnerContainer = document.querySelector('.spinner-border');
    if (typeof (spinnerContainer) != 'undefined' && spinnerContainer != null) {
        spinnerContainer.style.display = "block";
    }
}

function hideSpinner() {
    let spinnerContainer = document.querySelector('.spinner-border');
    if (typeof (spinnerContainer) != 'undefined' && spinnerContainer != null) {
        spinnerContainer.style.display = "none";
    }
}