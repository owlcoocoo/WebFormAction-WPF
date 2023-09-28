var args1, args2;
var ele = getElement(args1);
var event = document.createEvent('HTMLEvents');
event.initEvent('click', true, false);
ele.dispatchEvent(event);
event = document.createEvent('HTMLEvents');
event.initEvent('focus', true, false);
ele.dispatchEvent(event);
var nodeName = ele.nodeName.toUpperCase();
if (nodeName === 'BODY' || nodeName === 'DIV' || nodeName !== 'INPUT') {
    ele.innerHTML = args2;
} else {
    ele.value = args2;
}
event = document.createEvent('HTMLEvents');
event.initEvent('input', true, false);
ele.dispatchEvent(event);
event = document.createEvent('HTMLEvents');
event.initEvent('change', true, false);
ele.dispatchEvent(event);
event = document.createEvent('HTMLEvents');
event.initEvent('blur', true, false);
ele.dispatchEvent(event);