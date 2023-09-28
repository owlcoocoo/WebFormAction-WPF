var args1; var ele = getElement(args1);
var event = document.createEvent('HTMLEvents');
event.initEvent('mousedown', true, false);
ele.dispatchEvent(event);
ele.click();