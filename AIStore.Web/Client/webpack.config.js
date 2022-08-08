let CopyWebpackPlugin = require('copy-webpack-plugin');
const path = require('path');

module.exports = (config, options) => {
  let deployUrl = '/template/dist/';
  console.log(`\nURL for yours static resources : ~${deployUrl}\n`);

  config.module.rules.push({
    test: /\.s[ac]ss$/i,
    use: [
      {
        loader: 'sass-loader',
        options: {
          prependData: '$staticUrl: \'' + deployUrl + '\';',
        }
      }
    ]
  });

  config.plugins.push(
    new CopyWebpackPlugin([
      { from: '../wwwroot/images', to: 'images' },
      { from: '../wwwroot/fonts', to: 'fonts' }
    ])
  );

  config.module.rules.push({
    test: /\.m?js$/,
    /**
     * Exclude `node_modules` except the ones that need transpiling for IE11 compatibility.
     * Run `$ npx are-you-es5 check . -r` to get a list of those modules.
     */
    exclude : [
      /\bcore-js\b/,
      /\bwebpack\/buildin\b/
    ],
    use: {
      loader: 'babel-loader',
      options: {
        presets: [
          ['@babel/preset-env', {
            targets: {
              // Criteria for selecting browsers. See https://github.com/browserslist/browserslist
              browsers: ['> 0.5%', ' last 2 versions', ' Firefox ESR', ' not dead', ' IE 11']
            }
          }]
        ]
      }
    }
  });

  return config;
};
