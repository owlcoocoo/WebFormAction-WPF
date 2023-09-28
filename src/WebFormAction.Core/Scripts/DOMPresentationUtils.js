/*
 * Copyright (C) 2011 Google Inc.  All rights reserved.
 * Copyright (C) 2007, 2008 Apple Inc.  All rights reserved.
 * Copyright (C) 2008 Matt Lilek <webkit@mattlilek.com>
 * Copyright (C) 2009 Joseph Pecoraro
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1.  Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 * 2.  Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 * 3.  Neither the name of Apple Computer, Inc. ("Apple") nor the names of
 *     its contributors may be used to endorse or promote products derived
 *     from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY APPLE AND ITS CONTRIBUTORS "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL APPLE OR ITS CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
DOMPresentationUtils = {};
DOMPresentationUtils.DOMNodePathStep = class {
    constructor(value, optimized) {
        this.value = value;
        this.optimized = optimized || false;
    }
    toString() {
        return this.value;
    }
};
DOMPresentationUtils.nodeNameInCorrectCase = function (node) {
    var shadowRootType = node.shadowRootType; if (shadowRootType)
        return '#shadow-root (' + shadowRootType + ')'; if (!node.localName)
        return node.nodeName; if (node.localName.length !== node.nodeName.length)
        return node.nodeName; return node.localName;
};
DOMPresentationUtils.cssPath = function (node, optimized) {
    if (node.nodeType !== Node.ELEMENT_NODE)
        return ''; var steps = []; var contextNode = node; while (contextNode) {
            var step = DOMPresentationUtils._cssPathStep(contextNode, !!optimized, contextNode === node); if (!step)
                break; steps.push(step); if (step.optimized)
                break; contextNode = contextNode.parentNode;
        }
    steps.reverse(); return steps.join(' > ');
}; DOMPresentationUtils._cssPathStep = function (node, optimized, isTargetNode) {
    if (node.nodeType !== Node.ELEMENT_NODE)
        return null; var id = node.getAttribute('id'); if (optimized) {
            if (id)
                return new DOMPresentationUtils.DOMNodePathStep(idSelector(id), true); var nodeNameLower = node.nodeName.toLowerCase();
            if (nodeNameLower === 'body' || nodeNameLower === 'head' || nodeNameLower === 'html')
                return new DOMPresentationUtils.DOMNodePathStep(DOMPresentationUtils.nodeNameInCorrectCase(node), true);
        }
    var nodeName = DOMPresentationUtils.nodeNameInCorrectCase(node); if (id)
        return new DOMPresentationUtils.DOMNodePathStep(nodeName + idSelector(id), true); var parent = node.parentNode; if (!parent || parent.nodeType === Node.DOCUMENT_NODE)
        return new DOMPresentationUtils.DOMNodePathStep(nodeName, true); function prefixedElementClassNames(node) {
            var classAttribute = node.getAttribute('class'); if (!classAttribute)
                return []; return classAttribute.split(/\s+/g).filter(Boolean).map(function (name) { return '$' + name; });
        }
    function idSelector(id) { return '#' + escapeIdentifierIfNeeded(id); }
    function escapeIdentifierIfNeeded(ident) {
        if (isCSSIdentifier(ident))
            return ident; var shouldEscapeFirst = /^(?:[0-9]|-[0-9-]?)/.test(ident); var lastIndex = ident.length - 1; return ident.replace(/./g, function (c, i) { return ((shouldEscapeFirst && i === 0) || !isCSSIdentChar(c)) ? escapeAsciiChar(c, i === lastIndex) : c; });
    }
    function escapeAsciiChar(c, isLast) { return '\\' + toHexByte(c) + (isLast ? '' : ' '); }
    function toHexByte(c) {
        var hexByte = c.charCodeAt(0).toString(16); if (hexByte.length === 1)
            hexByte = '0' + hexByte; return hexByte;
    }
    function isCSSIdentChar(c) {
        if (/[a-zA-Z0-9_-]/.test(c))
            return true; return c.charCodeAt(0) >= 0xA0;
    }
    function isCSSIdentifier(value) { return /^-{0,2}[a-zA-Z_][a-zA-Z0-9_-]*$/.test(value); }
    var prefixedOwnClassNamesArray = prefixedElementClassNames(node); var needsClassNames = false; var needsNthChild = false; var ownIndex = -1; var elementIndex = -1; var siblings = parent.children; for (var i = 0; (ownIndex === -1 || !needsNthChild) && i < siblings.length; ++i) {
        var sibling = siblings[i]; if (sibling.nodeType !== Node.ELEMENT_NODE)
            continue; elementIndex += 1; if (sibling === node) { ownIndex = elementIndex; continue; }
        if (needsNthChild)
            continue; if (DOMPresentationUtils.nodeNameInCorrectCase(sibling) !== nodeName)
            continue; needsClassNames = true; var ownClassNames = new Set(prefixedOwnClassNamesArray); if (!ownClassNames.size) { needsNthChild = true; continue; }
        var siblingClassNamesArray = prefixedElementClassNames(sibling); for (var j = 0; j < siblingClassNamesArray.length; ++j) {
            var siblingClass = siblingClassNamesArray[j]; if (!ownClassNames.has(siblingClass))
                continue; ownClassNames.delete(siblingClass); if (!ownClassNames.size) { needsNthChild = true; break; }
        }
    }
    var result = nodeName; if (isTargetNode && nodeName.toLowerCase() === 'input' && node.getAttribute('type') && !node.getAttribute('id') && !node.getAttribute('class'))
        result += '[type="' + node.getAttribute('type') + '"]'; if (needsNthChild) { result += ':nth-child(' + (ownIndex + 1) + ')'; } else if (needsClassNames) {
            for (var prefixedName of prefixedOwnClassNamesArray)
                result += '.' + escapeIdentifierIfNeeded(prefixedName.substr(1));
        }
    return new DOMPresentationUtils.DOMNodePathStep(result, false);
};