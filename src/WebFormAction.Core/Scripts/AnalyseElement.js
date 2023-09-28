var sign_array = [];
var ele = document.elementFromPoint(MouseEventData.X, MouseEventData.Y);
while (ele) {
	var tagName = ele.tagName.toLowerCase();
	if (tagName === 'body')
		break;
	var sign = '';
	var array = [];
	for (var attr of ele.attributes) {
		var nodeName = attr.nodeName.toLowerCase();
		if (nodeName === 'style')
			continue;
		var node = '';
		if (nodeName === 'class') {
			if (!attr.nodeValue || attr.nodeValue === '')
				continue;
			node = tagName + '.' + attr.nodeValue.replace(/[ ]/g, '.');
		}
		else
			node = `${tagName}[${nodeName}="${attr.nodeValue}"]`;

		array.push(node);
	}
	if (array.length > 0)
		sign_array.push(array);
	ele = ele.parentNode;
}
sign_array;