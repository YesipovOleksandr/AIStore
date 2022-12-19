var scriptService = (function () {

    var service = {};

    ScriptStore = [
        { name: 'google-drive', src: 'https://apis.google.com/js/client.js' },
        { name: 'google-one-tap', src: 'https://accounts.google.com/gsi/client' },
        { id: 'dropboxjs', name: 'dropbox', src: 'https://www.dropbox.com/static/api/2/dropins.js', key: 'data-app-key', value: window.clientConfig.environmentconfig.dropboxapikey }
    ];

    var scripts = {};

    ScriptStore.forEach(function (script) {
        scripts[script.name] = {
            loaded: false,
            id: script.id,
            src: script.src,
            key: script.key,
            value: script.value,
        };
    });

    service.loadScript = function loadScript(name) {
        return new Promise(function (resolve, reject) {
            //resolve if already loaded
            if (scripts[name].loaded) {
                resolve({ script: name, loaded: true, status: 'Already Loaded' });
            }
            else {
                //load script
                var script = document.createElement('script');
                script.type = 'text/javascript';
                script.src = scripts[name].src;

                if (scripts[name].id) {
                    script.id = scripts[name].id;
                }

                if (scripts[name].key && scripts[name].value) {
                    script.setAttribute(scripts[name].key, scripts[name].value);
                }

                if (script.readyState) {  //IE
                    script.onreadystatechange = function () {
                        if (script.readyState === "loaded" || script.readyState === "complete") {
                            script.onreadystatechange = null;
                            scripts[name].loaded = true;
                            resolve({ script: name, loaded: true, status: 'Loaded' });
                        }
                    };
                } else {  //Others
                    script.onload = function () {
                        scripts[name].loaded = true;
                        resolve({ script: name, loaded: true, status: 'Loaded' });
                    };
                }
                script.onerror = function (error) { resolve({ script: name, loaded: false, status: 'Loaded' }); };
                document.getElementsByTagName('head')[0].appendChild(script);
            }
        });
    };
    return service;
}());