var args1, args2;
var ele = getElement(args1);
var array = args2.split('|');
if (ele.nodeName.toUpperCase() === 'SELECT') {
    n = Number(array[0]);
    if (ele.options[n].innerText !== array[1]) {
        n = -1;
        for (var i = 0; i < ele.options.length; i++) {
            if (ele.options[i].innerText === array[1]) {
                n = i;
                break;
            } 
        }
    }
    ele.selectedIndex = n;
    var event = document.createEvent('HTMLEvents');
    event.initEvent('change', true, false);
    ele.dispatchEvent(event);
}
else if (ele.nodeName.toUpperCase() === 'INPUT') {
    ele = ele.parentElement.nextElementSibling;
    n = Number(array[0]);
    while (ele.children[0] && ele.children[0].children[0] && ele.nodeName.toUpperCase() === 'DIV') ele = ele.children[0];
    while (ele.children.length === 1) ele = ele.children[0];
    ele = ele.children[n];
    while (ele.firstElementChild && ele.nodeName.toUpperCase() !== 'LI') ele = ele.firstElementChild;
    while (ele.firstElementChild && ele.firstElementChild.nodeName.toUpperCase() === 'A') ele = ele.firstElementChild;
    ele.click();
} else {
    n = Number(array[0]);
    while (ele.children[0] && ele.children[0].children[0] && ele.nodeName.toUpperCase() === 'DIV') ele = ele.children[0];
    while (ele.children.length === 1) ele = ele.children[0];
    ele = ele.children[n];
    while (ele.firstElementChild && ele.nodeName.toUpperCase() !== 'LI') ele = ele.firstElementChild;
    while (ele.firstElementChild && ele.firstElementChild.nodeName.toUpperCase() === 'A') ele = ele.firstElementChild;
    ele.click();
}