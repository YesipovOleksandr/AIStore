/// <binding BeforeBuild='build' />

'use strict';

const { series, parallel } = require('gulp');

var gulp = require('gulp');
var sass = require('gulp-sass')(require('sass'));
var sourcemaps = require('gulp-sourcemaps');
var rename = require('gulp-rename');
const babel = require("gulp-babel");
const concat = require('gulp-concat');
const minify = require('gulp-minify');

const BABEL_POLYFILL = './node_modules/@babel/polyfill/dist/polyfill.min.js';

var mainSectionScripts = ['./wwwroot/js/mainSection/angularHelper.js'];

function mainCss() {
    return gulp.src('./wwwroot/styles/pages/*/*.scss')
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        // .pipe(autoprefixer())
        .pipe(rename(function (path) {
            return {
                dirname: path.dirname.split('/').pop(),
                basename: path.basename,
                extname: ".css"
            };
        }))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('./wwwroot/dist/css/pages/'));
};

function minifyMainCss() {
    return gulp.src('./wwwroot/styles/pages/*/*.scss')
        .pipe(sass({ outputStyle: 'compressed' }).on('error', sass.logError))
        // .pipe(autoprefixer())
        .pipe(rename(function (path) {
            return {
                dirname: path.dirname.split('/').pop(),
                basename: path.basename,
                extname: ".css"
            };
        }))
        .pipe(gulp.dest('./wwwroot/dist/css/pages/'));
};

function CopyScripts() {
    return gulp.src('./wwwroot/js/**/*.js')
        .pipe(babel({
            presets: [['@babel/preset-env', { loose: true, modules: false }]]
        })
        )
        .pipe(rename(function (path) {
            path.extname = "-min" + path.extname;
        }))
        .pipe(gulp.dest('./wwwroot/dist/js/'))
        && gulp.src(BABEL_POLYFILL)
            .pipe(gulp.dest('./wwwroot/dist/js/'))
        && gulp.src(mainSectionScripts)
            .pipe(concat('mainSection.js', { 'newLine': '\n' }))
            .pipe(babel({
                presets: [['@babel/preset-env', { loose: true, modules: false }]]
            })
            )
            .pipe(rename(function (path) {
                path.extname = "-min" + path.extname;
            }))
            .pipe(gulp.dest('./wwwroot/dist/js/'));
};

function minifyAndCopyScripts() {
    return gulp.src('./wwwroot/js/**/*.js')
        .pipe(babel({
            presets: [['@babel/preset-env', { loose: true, modules: false }]]
        })
        )
        .pipe(minify({
            noSource: true
        }))
        .pipe(rename(function (path) {
            path.extname = path.extname;
        }))
        .pipe(gulp.dest('./wwwroot/dist/js/'))
        && gulp.src(mainSectionScripts)
            .pipe(concat('mainSection.js', { 'newLine': '\n' }))
            .pipe(babel({
                presets: [['@babel/preset-env', { loose: true, modules: false }]]
            })
            )
            .pipe(minify({
                noSource: true
            }))
            .pipe(rename(function (path) {
                path.extname = path.extname;
            }))
            .pipe(gulp.dest('./wwwroot/dist/js/'));
};

function CopyForms() {
    return gulp.src('./wwwroot/forms/*')
        .pipe(gulp.dest('./wwwroot/dist/forms/'));
}

exports.build = series(
    parallel(CopyScripts, mainCss, CopyForms)
);

exports.serverBuild = series(
    parallel(minifyAndCopyScripts, minifyMainCss, CopyForms)
);

gulp.task('sass:watch', function () {
    gulp.watch('./wwwroot/styles/**/**/*.scss', series(mainCss));
});