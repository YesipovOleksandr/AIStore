function applyAngular() {
    var angularRoot = document.createElement("app-root");
    angularRoot.classList.add('angular-section');
    angularRoot.style.display = 'none';
    angularRoot.innerHTML = window.angularMainTemplate;
    mainSection.parentElement.insertBefore(angularRoot, mainSection);

    var angularScripts = window.angularMainScripts.split('src="');

    for (var i = 1; i < angularScripts.length; ++i) {
        var my_awesome_script = document.createElement('script');

        my_awesome_script.setAttribute('src', angularScripts[i].slice(0, angularScripts[i].indexOf('"')));
        mainSection.parentElement.insertBefore(my_awesome_script, mainSection);
    }
}