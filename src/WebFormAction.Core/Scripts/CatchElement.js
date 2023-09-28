function getElementIndex(element, ele) {
    var index = -1;
    ele = ele.replace(/[|]/g, '\\|');
    for (var n = 0; n < $(ele).length; n++) {
        if (element === $(ele)[n]) {
            index = n;
            break;
        }
    }
    return index;
}
var element = document.elementFromPoint(mouseEvent.x, mouseEvent.y);
var str = '#';
var ele = $(element).attr('id');
var tmpEle;
var match = /(?=[^.])(\s)(?=\S)(?=[^.])/g;
if (!ele) {
    str = '.';
    ele = $(element).attr('class');
    if (ele)
        ele = ele.replace(match, '.');
}
if (ele) {
    ele = element.nodeName + str + ele;
    var n = getElementIndex(element, ele);
    if (n === -1) {
        tmpEle = element;
        while (tmpEle && tmpEle.parentNode) {
            if (tmpEle.parentNode.className) {
                ele = tmpEle.parentNode.nodeName + '.' + tmpEle.parentNode.className.replace(match, '.').replace(/[ ]/g, '') + ' ' + element.nodeName;
                n = getElementIndex(element, ele);
                break;
            }
            tmpEle = tmpEle.parentNode;
        }
    }
}
else {
    n = 0;
    var tag = element.nodeName.toUpperCase();
    if (tag === 'HTML' || tag === 'BODY') {
        ele = 'BODY';
    }
    else {
        tmpEle = element;
        while (tmpEle && tmpEle.parentNode) {
            if (tmpEle.parentNode.className) {
                ele = tmpEle.parentNode.nodeName + '.' + tmpEle.parentNode.className.replace(match, '.').replace(/[ ]/g, '') + ' ' + element.nodeName;
                n = getElementIndex(element, ele);
                break;
            }
            tmpEle = tmpEle.parentNode;
        }
    }
}
if (ele) {
    ele = ele.replace(/[|]/g, '\\\\|');
    str = ele + '|' + n;
    mouseEvent.elementSign = identifier + str;
}
if ($(element).html && $(element).html().replace(/[ ]/g, '') !== '') {
    str = element.nodeName + ':contains(\'' + $(element).html() + '\')';
    n = getElementIndex(element, str);
    str = str + '|' + n;
    if (n >= 0) {
        if (ele)
            mouseEvent.elementSign += '&' + identifier + str;
        else
            mouseEvent.elementSign = identifier + str;
    }
}