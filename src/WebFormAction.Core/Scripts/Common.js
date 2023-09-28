function getElement(args) {
    var ele = args.split('|')[1];
    return document.querySelector(ele);
}
function randomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    var num = Min + Math.floor(Rand * Range);
    return num;
}
