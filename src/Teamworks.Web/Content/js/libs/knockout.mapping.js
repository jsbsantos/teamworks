/// Knockout Mapping plugin v2.3.0
/// (c) 2012 Steven Sanderson, Roy Jacobs - http://knockoutjs.com/
/// License: MIT (http://www.opensource.org/licenses/mit-license.php)
(function(d) { "function" === typeof require && "object" === typeof exports && "object" === typeof module ? d(require("knockout"), exports) : "function" === typeof define && define.amd ? define(["knockout", "exports"], d) : d(ko, ko.mapping = { }); })(function(d, e) {

    function w(a, c) {
        for (var b in c)
            if (c.hasOwnProperty(b) && c[b])
                if (b && a[b] && "array" !== e.getType(a[b])) w(a[b], c[b]);
                else if ("array" === e.getType(a[b]) && "array" === e.getType(c[b])) {
                    for (var d = a, g = b, j = a[b], m = c[b], i = { }, k = j.length - 1; 0 <= k; --k) i[j[k]] = j[k];
                    for (k = m.length - 1; 0 <= k; --k)
                        i[m[k]] =
                            m[k];
                    j = [];
                    m = void 0;
                    for (m in i) j.push(i[m]);
                    d[g] = j;
                } else a[b] = c[b];
    }

    function B(a, c) {
        var b = { };
        w(b, a);
        w(b, c);
        return b;
    }

    function y(a, c) {
        options = B({ }, a);
        for (var b = J.length - 1; 0 <= b; b--) {
            var d = J[b];
            options[d] && (options[""] instanceof Object || (options[""] = { }), options[""][d] = options[d], delete options[d]);
        }
        c && (options.ignore = p(c.ignore, options.ignore), options.include = p(c.include, options.include), options.copy = p(c.copy, options.copy));
        options.ignore = p(options.ignore, h.ignore);
        options.include = p(options.include, h.include);
        options.copy = p(options.copy, h.copy);
        options.mappedProperties = options.mappedProperties || { };
        return options;
    }

    function p(a, c) {
        "array" !== e.getType(a) && (a = "undefined" === e.getType(a) ? [] : [a]);
        "array" !== e.getType(c) && (c = "undefined" === e.getType(c) ? [] : [c]);
        return d.utils.arrayGetDistinctValues(a.concat(c));
    }

    function C(a, c, b, f, g, j) {
        var m = "array" === e.getType(d.utils.unwrapObservable(c)), j = j || "";
        if (e.isMapped(a)) var i = d.utils.unwrapObservable(a)[q], b = B(i, b);
        var k = function() { return b[f] && b[f].create instanceof Function; },
            H = function(a) {
                var e = D, m = d.dependentObservable;
                d.dependentObservable = function(a, b, c) {
                    c = c || { };
                    a && "object" == typeof a && (c = a);
                    var f = c.deferEvaluation, K = !1;
                    c.deferEvaluation = !0;
                    a = new E(a, b, c);
                    if (!f) {
                        var g = a, f = d.dependentObservable;
                        d.dependentObservable = E;
                        a = d.isWriteableObservable(g);
                        d.dependentObservable = f;
                        a = E({
                            read: function() {
                                K || (d.utils.arrayRemoveItem(e, g), K = !0);
                                return g.apply(g, arguments);
                            },
                            write: a && function(a) { return g(a); },
                            deferEvaluation: !0
                        });
                        e.push(a);
                    }
                    return a;
                };
                d.dependentObservable.fn = E.fn;
                d.computed =
                    d.dependentObservable;
                a = d.utils.unwrapObservable(g) instanceof Array ? b[f].create({ data: a || c, parent: g, skip: L }) : b[f].create({ data: a || c, parent: g });
                d.dependentObservable = m;
                d.computed = d.dependentObservable;
                return a;
            }, h = function() { return b[f] && b[f].update instanceof Function; }, t = function(a, e) {
                var m = { data: e || c, parent: g, target: d.utils.unwrapObservable(a) };
                d.isWriteableObservable(a) && (m.observable = a);
                return b[f].update(m);
            };
        if (i = F.get(c)) return i;
        f = f || "";
        if (m) {
            var m = [], r = !1, l = function(a) { return a; };
            b[f] && b[f].key &&
                (l = b[f].key, r = !0);
            d.isObservable(a) || (a = d.observableArray([]), a.mappedRemove = function(b) {
                var c = typeof b == "function" ? b : function(a) { return a === l(b); };
                return a.remove(function(a) { return c(l(a)); });
            }, a.mappedRemoveAll = function(b) {
                var c = z(b, l);
                return a.remove(function(a) { return d.utils.arrayIndexOf(c, l(a)) != -1; });
            }, a.mappedDestroy = function(b) {
                var c = typeof b == "function" ? b : function(a) { return a === l(b); };
                return a.destroy(function(a) { return c(l(a)); });
            }, a.mappedDestroyAll = function(b) {
                var c = z(b, l);
                return a.destroy(function(a) {
                    return d.utils.arrayIndexOf(c,
                        l(a)) != -1;
                });
            }, a.mappedIndexOf = function(b) {
                var c = z(a(), l), b = l(b);
                return d.utils.arrayIndexOf(c, b);
            }, a.mappedCreate = function(b) {
                if (a.mappedIndexOf(b) !== -1) throw Error("There already is an object with the key that you specified.");
                var c = k() ? H(b) : b;
                if (h()) {
                    b = t(c, b);
                    d.isWriteableObservable(c) ? c(b) : c = b;
                }
                a.push(c);
                return c;
            });
            var i = z(d.utils.unwrapObservable(a), l).sort(), n = z(c, l);
            r && n.sort();
            var r = d.utils.compareArrays(i, n), i = { }, p, x = d.utils.unwrapObservable(c), v = { }, w = !0, n = 0;
            for (p = x.length; n < p; n++) {
                var o = l(x[n]);
                if (void 0 === o || o instanceof Object) {
                    w = !1;
                    break;
                }
                v[o] = x[n];
            }
            var x = [], y = 0, n = 0;
            for (p = r.length; n < p; n++) {
                var o = r[n], s, u = j + "[" + n + "]";
                switch (o.status) {
                case "added":
                    var A = w ? v[o.value] : G(d.utils.unwrapObservable(c), o.value, l);
                    s = C(void 0, A, b, f, a, u);
                    k() || (s = d.utils.unwrapObservable(s));
                    u = M(d.utils.unwrapObservable(c), A, i);
                    s === L ? y++ : x[u - y] = s;
                    i[u] = !0;
                    break;
                case "retained":
                    A = w ? v[o.value] : G(d.utils.unwrapObservable(c), o.value, l);
                    s = G(a, o.value, l);
                    C(s, A, b, f, a, u);
                    u = M(d.utils.unwrapObservable(c), A, i);
                    x[u] = s;
                    i[u] = !0;
                    break;
                case "deleted":
                    s = G(a, o.value, l);
                }
                m.push({ event: o.status, item: s });
            }
            a(x);
            b[f] && b[f].arrayChanged && d.utils.arrayForEach(m, function(a) { b[f].arrayChanged(a.event, a.item); });
        } else if (N(c)) {
            a = d.utils.unwrapObservable(a);
            if (!a) {
                if (k()) return r = H(), h() && (r = t(r)), r;
                if (h()) return t(r);
                a = { };
            }
            h() && (a = t(a));
            F.save(c, a);
            O(c, function(f) {
                var e = j.length ? j + "." + f : f;
                if (-1 == d.utils.arrayIndexOf(b.ignore, e))
                    if (-1 != d.utils.arrayIndexOf(b.copy, e)) a[f] = c[f];
                    else {
                        var g = F.get(c[f]) || C(a[f], c[f], b, f, a, e);
                        if (d.isWriteableObservable(a[f])) a[f](d.utils.unwrapObservable(g));
                        else a[f] = g;
                        b.mappedProperties[e] = !0;
                    }
            });
        } else
            switch (e.getType(c)) {
            case "function":
                h() ? d.isWriteableObservable(c) ? (c(t(c)), a = c) : a = t(c) : a = c;
                break;
            default:
                d.isWriteableObservable(a) ? h() ? a(t(a)) : a(d.utils.unwrapObservable(c)) : (a = k() ? H() : d.observable(d.utils.unwrapObservable(c)), h() && a(t(a)));
            }
        return a;
    }

    function M(a, c, b) {
        for (var d = 0, e = a.length; d < e; d++) if (!0 !== b[d] && a[d] === c) return d;
        return null;
    }

    function P(a, c) {
        var b;
        c && (b = c(a));
        "undefined" === e.getType(b) && (b = a);
        return d.utils.unwrapObservable(b);
    }

    function G(a,
        c, b) {
        for (var a = d.utils.unwrapObservable(a), f = 0, e = a.length; f < e; f++) {
            var j = a[f];
            if (P(j, b) === c) return j;
        }
        throw Error("When calling ko.update*, the key '" + c + "' was not found!");
    }

    function z(a, c) {
        return d.utils.arrayMap(d.utils.unwrapObservable(a), function(a) { return c ? P(a, c) : a; });
    }

    function O(a, c) {
        if ("array" === e.getType(a)) for (var b = 0; b < a.length; b++) c(b);
        else for (b in a) c(b);
    }

    function N(a) {
        var c = e.getType(a);
        return ("object" === c || "array" === c) && null !== a;
    }

    function R() {
        var a = [], c = [];
        this.save = function(b, f) {
            var e =
                d.utils.arrayIndexOf(a, b);
            0 <= e ? c[e] = f : (a.push(b), c.push(f));
        };
        this.get = function(b) {
            b = d.utils.arrayIndexOf(a, b);
            return 0 <= b ? c[b] : void 0;
        };
    }

    function Q() {
        var a = { }, c = function(b) {
            var c;
            try {
                c = JSON.stringify(b);
            } catch(d) {
                c = "$$$";
            }
            b = a[c];
            void 0 === b && (b = new R, a[c] = b);
            return b;
        };
        this.save = function(a, d) { c(a).save(a, d); };
        this.get = function(a) { return c(a).get(a); };
    }

    var q = "__ko_mapping__", E = d.dependentObservable, I = 0, D, F, J = ["create", "update", "key", "arrayChanged"], L = { }, v = { include: ["_destroy"], ignore: [], copy: [] }, h = v;
    e.isMapped =
        function(a) { return (a = d.utils.unwrapObservable(a)) && a[q]; };
    e.fromJS = function(a) {
        if (0 == arguments.length) throw Error("When calling ko.fromJS, pass the object you want to convert.");
        window.setTimeout(function() { I = 0; }, 0);
        I++ || (D = [], F = new Q);
        var c, b;
        2 == arguments.length && (arguments[1][q] ? b = arguments[1] : c = arguments[1]);
        3 == arguments.length && (c = arguments[1], b = arguments[2]);
        b && (c = B(c, b[q]));
        c = y(c);
        var d = C(b, a, c);
        b && (d = b);
        --I || window.setTimeout(function() {
            for (; D.length;) {
                var a = D.pop();
                a && a();
            }
        }, 0);
        d[q] = B(d[q], c);
        return d;
    };
    e.fromJSON = function(a) {
        var c = d.utils.parseJson(a);
        arguments[0] = c;
        return e.fromJS.apply(this, arguments);
    };
    e.updateFromJS = function() { throw Error("ko.mapping.updateFromJS, use ko.mapping.fromJS instead. Please note that the order of parameters is different!"); };
    e.updateFromJSON = function() { throw Error("ko.mapping.updateFromJSON, use ko.mapping.fromJSON instead. Please note that the order of parameters is different!"); };
    e.toJS = function(a, c) {
        h || e.resetDefaultOptions();
        if (0 == arguments.length) throw Error("When calling ko.mapping.toJS, pass the object you want to convert.");
        if ("array" !== e.getType(h.ignore)) throw Error("ko.mapping.defaultOptions().ignore should be an array.");
        if ("array" !== e.getType(h.include)) throw Error("ko.mapping.defaultOptions().include should be an array.");
        if ("array" !== e.getType(h.copy)) throw Error("ko.mapping.defaultOptions().copy should be an array.");
        c = y(c, a[q]);
        return e.visitModel(a, function(a) { return d.utils.unwrapObservable(a); }, c);
    };
    e.toJSON = function(a, c) {
        var b = e.toJS(a, c);
        return d.utils.stringifyJson(b);
    };
    e.defaultOptions = function() {
        if (0 < arguments.length)
            h =
                arguments[0];
        else return h;
    };
    e.resetDefaultOptions = function() { h = { include: v.include.slice(0), ignore: v.ignore.slice(0), copy: v.copy.slice(0) }; };
    e.getType = function(a) {
        if (a && "object" === typeof a) {
            if (a.constructor == (new Date).constructor) return "date";
            if ("[object Array]" === Object.prototype.toString.call(a)) return "array";
        }
        return typeof a;
    };
    e.visitModel = function(a, c, b) {
        b = b || { };
        b.visitedObjects = b.visitedObjects || new Q;
        var f, g = d.utils.unwrapObservable(a);
        if (N(g))
            b = y(b, g[q]), c(a, b.parentName), f = "array" === e.getType(g) ?
                [] : { };
        else return c(a, b.parentName);
        b.visitedObjects.save(a, f);
        var j = b.parentName;
        O(g, function(a) {
            if (!(b.ignore && -1 != d.utils.arrayIndexOf(b.ignore, a))) {
                var i = g[a], k = b, h = j || "";
                "array" === e.getType(g) ? j && (h += "[" + a + "]") : (j && (h += "."), h += a);
                k.parentName = h;
                if (!(-1 === d.utils.arrayIndexOf(b.copy, a) && -1 === d.utils.arrayIndexOf(b.include, a) && g[q] && g[q].mappedProperties && !g[q].mappedProperties[a] && "array" !== e.getType(g)))
                    switch (e.getType(d.utils.unwrapObservable(i))) {
                    case "object":
                    case "array":
                    case "undefined":
                        k =
                            b.visitedObjects.get(i);
                        f[a] = "undefined" !== e.getType(k) ? k : e.visitModel(i, c, b);
                        break;
                    default:
                        f[a] = c(i, b.parentName);
                    }
            }
        });
        return f;
    };
});