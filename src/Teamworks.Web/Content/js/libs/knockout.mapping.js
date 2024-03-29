/// Knockout Mapping plugin v2.3.1
/// (c) 2012 Steven Sanderson, Roy Jacobs - http://knockoutjs.com/
/// License: MIT (http://www.opensource.org/licenses/mit-license.php)
(function (d) { "function" === typeof require && "object" === typeof exports && "object" === typeof module ? d(require("knockout"), exports) : "function" === typeof define && define.amd ? define(["knockout", "exports"], d) : d(ko, ko.mapping = {}) })(function (d, e) {
    function v(b, c) {
        for (var a in c) if (c.hasOwnProperty(a) && c[a]) if (a && b[a] && "array" !== e.getType(b[a])) v(b[a], c[a]); else if ("array" === e.getType(b[a]) && "array" === e.getType(c[a])) {
            for (var d = b, g = a, j = b[a], q = c[a], s = {}, h = j.length - 1; 0 <= h; --h) s[j[h]] = j[h]; for (h = q.length - 1; 0 <= h; --h) s[q[h]] =
q[h]; j = []; q = void 0; for (q in s) j.push(s[q]); d[g] = j
        } else b[a] = c[a]
    } function C(b, c) { var a = {}; v(a, b); v(a, c); return a } function w(b, c) { for (var a = C({}, b), d = J.length - 1; 0 <= d; d--) { var e = J[d]; a[e] && (a[""] instanceof Object || (a[""] = {}), a[""][e] = a[e], delete a[e]) } c && (a.ignore = i(c.ignore, a.ignore), a.include = i(c.include, a.include), a.copy = i(c.copy, a.copy)); a.ignore = i(a.ignore, k.ignore); a.include = i(a.include, k.include); a.copy = i(a.copy, k.copy); a.mappedProperties = a.mappedProperties || {}; return a } function i(b, c) {
        "array" !==
e.getType(b) && (b = "undefined" === e.getType(b) ? [] : [b]); "array" !== e.getType(c) && (c = "undefined" === e.getType(c) ? [] : [c]); return d.utils.arrayGetDistinctValues(b.concat(c))
    } function D(b, c, a, f, g, j, q) {
        var s = "array" === e.getType(d.utils.unwrapObservable(c)), j = j || ""; if (e.isMapped(b)) var h = d.utils.unwrapObservable(b)[p], a = C(h, a); var E = function () { return a[f] && a[f].create instanceof Function }, k = function (b) {
            var e = F, h = d.dependentObservable; d.dependentObservable = function (a, b, c) {
                c = c || {}; a && "object" == typeof a && (c = a);
                var f = c.deferEvaluation, K = !1; c.deferEvaluation = !0; a = new G(a, b, c); if (!f) { var g = a, f = d.dependentObservable; d.dependentObservable = G; a = d.isWriteableObservable(g); d.dependentObservable = f; a = G({ read: function () { K || (d.utils.arrayRemoveItem(e, g), K = !0); return g.apply(g, arguments) }, write: a && function (a) { return g(a) }, deferEvaluation: !0 }); e.push(a) } return a
            }; d.dependentObservable.fn = G.fn; d.computed = d.dependentObservable; b = d.utils.unwrapObservable(g) instanceof Array ? a[f].create({ data: b || c, parent: q, skip: L }) : a[f].create({ data: b ||
c, parent: q
            }); d.dependentObservable = h; d.computed = d.dependentObservable; return b
        }, i = function () { return a[f] && a[f].update instanceof Function }, x = function (b, e) { var g = { data: e || c, parent: q, target: d.utils.unwrapObservable(b) }; d.isWriteableObservable(b) && (g.observable = b); return a[f].update(g) }; if (h = H.get(c)) return h; f = f || ""; if (s) {
            var s = [], r = !1, l = function (a) { return a }; a[f] && a[f].key && (l = a[f].key, r = !0); d.isObservable(b) || (b = d.observableArray([]), b.mappedRemove = function (a) {
                var c = typeof a == "function" ? a : function (b) {
                    return b ===
l(a)
                }; return b.remove(function (a) { return c(l(a)) })
            }, b.mappedRemoveAll = function (a) { var c = A(a, l); return b.remove(function (a) { return d.utils.arrayIndexOf(c, l(a)) != -1 }) }, b.mappedDestroy = function (a) { var c = typeof a == "function" ? a : function (b) { return b === l(a) }; return b.destroy(function (a) { return c(l(a)) }) }, b.mappedDestroyAll = function (a) { var c = A(a, l); return b.destroy(function (a) { return d.utils.arrayIndexOf(c, l(a)) != -1 }) }, b.mappedIndexOf = function (a) { var c = A(b(), l), a = l(a); return d.utils.arrayIndexOf(c, a) }, b.mappedCreate =
function (a) { if (b.mappedIndexOf(a) !== -1) throw Error("There already is an object with the key that you specified."); var c = E() ? k(a) : a; if (i()) { a = x(c, a); d.isWriteableObservable(c) ? c(a) : c = a } b.push(c); return c }); var h = A(d.utils.unwrapObservable(b), l).sort(), n = A(c, l); r && n.sort(); var r = d.utils.compareArrays(h, n), h = {}, t, y = d.utils.unwrapObservable(c), v = {}, w = !0, n = 0; for (t = y.length; n < t; n++) { var o = l(y[n]); if (void 0 === o || o instanceof Object) { w = !1; break } v[o] = y[n] } var y = [], z = 0, n = 0; for (t = r.length; n < t; n++) {
                var o = r[n],
m, u = j + "[" + n + "]"; switch (o.status) { case "added": var B = w ? v[o.value] : I(d.utils.unwrapObservable(c), o.value, l); m = D(void 0, B, a, f, b, u, g); E() || (m = d.utils.unwrapObservable(m)); u = M(d.utils.unwrapObservable(c), B, h); m === L ? z++ : y[u - z] = m; h[u] = !0; break; case "retained": B = w ? v[o.value] : I(d.utils.unwrapObservable(c), o.value, l); m = I(b, o.value, l); D(m, B, a, f, b, u, g); u = M(d.utils.unwrapObservable(c), B, h); y[u] = m; h[u] = !0; break; case "deleted": m = I(b, o.value, l) } s.push({ event: o.status, item: m })
            } b(y); a[f] && a[f].arrayChanged && d.utils.arrayForEach(s,
function (b) { a[f].arrayChanged(b.event, b.item) })
        } else if (N(c)) { b = d.utils.unwrapObservable(b); if (!b) { if (E()) return r = k(), i() && (r = x(r)), r; if (i()) return x(r); b = {} } i() && (b = x(b)); H.save(c, b); O(c, function (f) { var e = j.length ? j + "." + f : f; if (-1 == d.utils.arrayIndexOf(a.ignore, e)) if (-1 != d.utils.arrayIndexOf(a.copy, e)) b[f] = c[f]; else { var g = H.get(c[f]), h = D(b[f], c[f], a, f, b, e, b), g = g || h; if (d.isWriteableObservable(b[f])) b[f](d.utils.unwrapObservable(g)); else b[f] = g; a.mappedProperties[e] = !0 } }) } else switch (e.getType(c)) {
            case "function": i() ?
d.isWriteableObservable(c) ? (c(x(c)), b = c) : b = x(c) : b = c; break; default: if (d.isWriteableObservable(b)) return m = i() ? x(b) : d.utils.unwrapObservable(c), b(m), m; b = E() ? k() : d.observable(d.utils.unwrapObservable(c))
        } return b
    } function M(b, c, a) { for (var d = 0, e = b.length; d < e; d++) if (!0 !== a[d] && b[d] === c) return d; return null } function P(b, c) { var a; c && (a = c(b)); "undefined" === e.getType(a) && (a = b); return d.utils.unwrapObservable(a) } function I(b, c, a) {
        for (var b = d.utils.unwrapObservable(b), f = 0, e = b.length; f < e; f++) {
            var j = b[f]; if (P(j,
a) === c) return j
        } throw Error("When calling ko.update*, the key '" + c + "' was not found!");
    } function A(b, c) { return d.utils.arrayMap(d.utils.unwrapObservable(b), function (a) { return c ? P(a, c) : a }) } function O(b, c) { if ("array" === e.getType(b)) for (var a = 0; a < b.length; a++) c(a); else for (a in b) c(a) } function N(b) { var c = e.getType(b); return ("object" === c || "array" === c) && null !== b } function R() {
        var b = [], c = []; this.save = function (a, f) { var e = d.utils.arrayIndexOf(b, a); 0 <= e ? c[e] = f : (b.push(a), c.push(f)) }; this.get = function (a) {
            a =
d.utils.arrayIndexOf(b, a); return 0 <= a ? c[a] : void 0
        } 
    } function Q() { var b = {}, c = function (a) { var c; try { c = a } catch (d) { c = "$$$" } a = b[c]; void 0 === a && (a = new R, b[c] = a); return a }; this.save = function (a, b) { c(a).save(a, b) }; this.get = function (a) { return c(a).get(a) } } var p = "__ko_mapping__", G = d.dependentObservable, z = 0, F, H, J = ["create", "update", "key", "arrayChanged"], L = {}, t = { include: ["_destroy"], ignore: [], copy: [] }, k = t; e.isMapped = function (b) { return (b = d.utils.unwrapObservable(b)) && b[p] }; e.fromJS = function (b) {
        if (0 == arguments.length) throw Error("When calling ko.fromJS, pass the object you want to convert.");
        window.setTimeout(function () { z = 0 }, 0); z++ || (F = [], H = new Q); var c, a; 2 == arguments.length && (arguments[1][p] ? a = arguments[1] : c = arguments[1]); 3 == arguments.length && (c = arguments[1], a = arguments[2]); a && (c = C(c, a[p])); c = w(c); var d = D(a, b, c); a && (d = a); --z || window.setTimeout(function () { for (; F.length; ) { var a = F.pop(); a && a() } }, 0); d[p] = C(d[p], c); return d
    }; e.fromJSON = function (b) { var c = d.utils.parseJson(b); arguments[0] = c; return e.fromJS.apply(this, arguments) }; e.updateFromJS = function () {
        throw Error("ko.mapping.updateFromJS, use ko.mapping.fromJS instead. Please note that the order of parameters is different!");
    }; e.updateFromJSON = function () { throw Error("ko.mapping.updateFromJSON, use ko.mapping.fromJSON instead. Please note that the order of parameters is different!"); }; e.toJS = function (b, c) {
        k || e.resetDefaultOptions(); if (0 == arguments.length) throw Error("When calling ko.mapping.toJS, pass the object you want to convert."); if ("array" !== e.getType(k.ignore)) throw Error("ko.mapping.defaultOptions().ignore should be an array."); if ("array" !== e.getType(k.include)) throw Error("ko.mapping.defaultOptions().include should be an array.");
        if ("array" !== e.getType(k.copy)) throw Error("ko.mapping.defaultOptions().copy should be an array."); c = w(c, b[p]); return e.visitModel(b, function (a) { return d.utils.unwrapObservable(a) }, c)
    }; e.toJSON = function (b, c) { var a = e.toJS(b, c); return d.utils.stringifyJson(a) }; e.defaultOptions = function () { if (0 < arguments.length) k = arguments[0]; else return k }; e.resetDefaultOptions = function () { k = { include: t.include.slice(0), ignore: t.ignore.slice(0), copy: t.copy.slice(0)} }; e.getType = function (b) {
        if (b && "object" === typeof b) {
            if (b.constructor ==
(new Date).constructor) return "date"; if ("[object Array]" === Object.prototype.toString.call(b)) return "array"
        } return typeof b
    }; e.visitModel = function (b, c, a) {
        a = a || {}; a.visitedObjects = a.visitedObjects || new Q; var f, g = d.utils.unwrapObservable(b); if (N(g)) a = w(a, g[p]), c(b, a.parentName), f = "array" === e.getType(g) ? [] : {}; else return c(b, a.parentName); a.visitedObjects.save(b, f); var j = a.parentName; O(g, function (b) {
            if (!(a.ignore && -1 != d.utils.arrayIndexOf(a.ignore, b))) {
                var k = g[b], h = a, i = j || ""; "array" === e.getType(g) ? j &&
(i += "[" + b + "]") : (j && (i += "."), i += b); h.parentName = i; if (!(-1 === d.utils.arrayIndexOf(a.copy, b) && -1 === d.utils.arrayIndexOf(a.include, b) && g[p] && g[p].mappedProperties && !g[p].mappedProperties[b] && "array" !== e.getType(g))) switch (e.getType(d.utils.unwrapObservable(k))) { case "object": case "array": case "undefined": h = a.visitedObjects.get(k); f[b] = "undefined" !== e.getType(h) ? h : e.visitModel(k, c, a); break; default: f[b] = c(k, a.parentName) } 
            } 
        }); return f
    } 
});