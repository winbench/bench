var os = require('os');
var gulp = require('gulp');
var concat = require('gulp-concat');
var less = require('gulp-less');
var cleanCSS = require('gulp-clean-css');
var mdquery = require('mdquery').transform;
var textTransformation = require('gulp-text-simple');

var highlightTheme = 'github';

var rootHeadlineText = "TMPROOT";

var insertRootHeadline = textTransformation(function (txt) {
    var headline = os.EOL + os.EOL + '# ' + rootHeadlineText;
    var frontMatterBegin = txt.indexOf('+++');
    var frontMatterEnd = frontMatterBegin >= 0 ? txt.indexOf('+++', frontMatterBegin + 3) + 3 : -1;
    var insertionPoint = Math.max(frontMatterEnd, 0);
    return txt.substring(0, insertionPoint) + headline + txt.substring(insertionPoint);
});

var removeRootHeadline = textTransformation(function (txt) {
    var pattern = new RegExp(os.EOL + '#\\s+' + rootHeadlineText + os.EOL);
    return txt.replace(pattern, '');
});

gulp.task('preprocess-markdown', function () {
    return gulp.src('src-content/**/*.md')
        .pipe(insertRootHeadline())
        .pipe(mdquery())
        .pipe(removeRootHeadline())
        .pipe(gulp.dest('content'));
});

gulp.task('copy-fonts', function () {
return     gulp.src(['bower_components/font-awesome/fonts/*'])
        .pipe(gulp.dest('static/fonts'));
});

gulp.task('build-less', function () {
    return gulp.src('./src-static/css/*.less')
        .pipe(less())
        .pipe(concat('custom.css'))
        .pipe(gulp.dest('./src-static/css/'));
});

gulp.task('build-css', ['build-less'], function () {
    return gulp.src(['./bower_components/pure/base.css',
                     './bower_components/pure/grids-responsive.css',
                     './bower_components/pure/menus-core.css',
                     './bower_components/font-awesome/css/font-awesome.css',
                     './bower_components/highlight/src/styles/' + highlightTheme + '.css',
                     './themes/blackburn/static/css/side-menu.css',
                     './themes/blackburn/static/css/blackburn.css',
                     './src-static/css/custom.css'])
        .pipe(concat('style.min.css'))
        .pipe(cleanCSS())
        .pipe(gulp.dest('static/css/'));
});

gulp.task('default', ['preprocess-markdown', 'copy-fonts', 'build-css']);
