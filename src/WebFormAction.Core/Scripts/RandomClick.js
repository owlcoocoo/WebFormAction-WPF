var args1;
var ele = getElement(args1);
var n = ele.children.length;
n = randomNum(1, n);
ele = ele.children[n];
while (ele.firstElementChild)
    ele = ele.firstElementChild;
ele.click();