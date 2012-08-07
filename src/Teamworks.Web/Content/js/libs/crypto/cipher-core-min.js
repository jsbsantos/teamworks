/*
CryptoJS v3.0.2
code.google.com/p/crypto-js
(c) 2009-2012 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
CryptoJS.lib.Cipher || function(r) {
    var f = CryptoJS, e = f.lib, i = e.Base, j = e.WordArray, o = e.BufferedBlockAlgorithm, p = f.enc.Base64, s = f.algo.EvpKDF, l = e.Cipher = o.extend({
        cfg: i.extend(),
        createEncryptor: function(a, b) { return this.create(this._ENC_XFORM_MODE, a, b); },
        createDecryptor: function(a, b) { return this.create(this._DEC_XFORM_MODE, a, b); },
        init: function(a, b, c) {
            this.cfg = this.cfg.extend(c);
            this._xformMode = a;
            this._key = b;
            this.reset();
        },
        reset: function() {
            o.reset.call(this);
            this._doReset();
        },
        process: function(a) {
            this._append(a);
            return this._process();
        },
        finalize: function(a) {
            a && this._append(a);
            return this._doFinalize();
        },
        keySize: 4,
        ivSize: 4,
        _ENC_XFORM_MODE: 1,
        _DEC_XFORM_MODE: 2,
        _createHelper: function() { return function(a) { return { encrypt: function(b, c, d) { return ("string" == typeof c ? q : g).encrypt(a, b, c, d); }, decrypt: function(b, c, d) { return ("string" == typeof c ? q : g).decrypt(a, b, c, d); } }; }; }()
    });
    e.StreamCipher = l.extend({ _doFinalize: function() { return this._process(!0); }, blockSize: 1 });
    var k = f.mode = { }, t = e.BlockCipherMode = i.extend({
        createEncryptor: function(a,
            b) {
            return this.Encryptor.create(a, b);
        },
        createDecryptor: function(a, b) { return this.Decryptor.create(a, b); },
        init: function(a, b) {
            this._cipher = a;
            this._iv = b;
        }
    }), k = k.CBC = function() {

        function a(a, b, m) {
            var h = this._iv;
            h ? this._iv = r : h = this._prevBlock;
            for (var e = 0; e < m; e++) a[b + e] ^= h[e];
        }

        var b = t.extend();
        b.Encryptor = b.extend({
            processBlock: function(b, d) {
                var m = this._cipher, e = m.blockSize;
                a.call(this, b, d, e);
                m.encryptBlock(b, d);
                this._prevBlock = b.slice(d, d + e);
            }
        });
        b.Decryptor = b.extend({
            processBlock: function(b, d) {
                var e = this._cipher,
                    h = e.blockSize, f = b.slice(d, d + h);
                e.decryptBlock(b, d);
                a.call(this, b, d, h);
                this._prevBlock = f;
            }
        });
        return b;
    }(), u = (f.pad = { }).Pkcs7 = {
        pad: function(a, b) {
            for (var c = 4 * b, c = c - a.sigBytes % c, d = c << 24 | c << 16 | c << 8 | c, e = [], f = 0; f < c; f += 4) e.push(d);
            c = j.create(e, c);
            a.concat(c);
        },
        unpad: function(a) { a.sigBytes -= a.words[a.sigBytes - 1 >>> 2] & 255; }
    };
    e.BlockCipher = l.extend({
        cfg: l.cfg.extend({ mode: k, padding: u }),
        reset: function() {
            l.reset.call(this);
            var a = this.cfg, b = a.iv, a = a.mode;
            if (this._xformMode == this._ENC_XFORM_MODE) var c = a.createEncryptor;
            else c = a.createDecryptor, this._minBufferSize = 1;
            this._mode = c.call(a, this, b && b.words);
        },
        _doProcessBlock: function(a, b) { this._mode.processBlock(a, b); },
        _doFinalize: function() {
            var a = this.cfg.padding;
            if (this._xformMode == this._ENC_XFORM_MODE) {
                a.pad(this._data, this.blockSize);
                var b = this._process(!0);
            } else b = this._process(!0), a.unpad(b);
            return b;
        },
        blockSize: 4
    });
    var n = e.CipherParams = i.extend({ init: function(a) { this.mixIn(a); }, toString: function(a) { return (a || this.formatter).stringify(this); } }), k = (f.format = { }).OpenSSL =
        {
            stringify: function(a) {
                var b = a.ciphertext, a = a.salt, b = (a ? j.create([1398893684, 1701076831]).concat(a).concat(b) : b).toString(p);
                return b = b.replace( /(.{64})/g , "$1\n");
            },
            parse: function(a) {
                var a = p.parse(a), b = a.words;
                if (1398893684 == b[0] && 1701076831 == b[1]) {
                    var c = j.create(b.slice(2, 4));
                    b.splice(0, 4);
                    a.sigBytes -= 16;
                }
                return n.create({ ciphertext: a, salt: c });
            }
        }, g = e.SerializableCipher = i.extend({
            cfg: i.extend({ format: k }),
            encrypt: function(a, b, c, d) {
                var d = this.cfg.extend(d), e = a.createEncryptor(c, d), b = e.finalize(b), e = e.cfg;
                return n.create({ ciphertext: b, key: c, iv: e.iv, algorithm: a, mode: e.mode, padding: e.padding, blockSize: a.blockSize, formatter: d.format });
            },
            decrypt: function(a, b, c, d) {
                d = this.cfg.extend(d);
                b = this._parse(b, d.format);
                return a.createDecryptor(c, d).finalize(b.ciphertext);
            },
            _parse: function(a, b) { return "string" == typeof a ? b.parse(a) : a; }
        }), f = (f.kdf = { }).OpenSSL = {
            compute: function(a, b, c, d) {
                d || (d = j.random(8));
                a = s.create({ keySize: b + c }).compute(a, d);
                c = j.create(a.words.slice(b), 4 * c);
                a.sigBytes = 4 * b;
                return n.create({
                    key: a,
                    iv: c,
                    salt: d
                });
            }
        }, q = e.PasswordBasedCipher = g.extend({
            cfg: g.cfg.extend({ kdf: f }),
            encrypt: function(a, b, c, d) {
                d = this.cfg.extend(d);
                c = d.kdf.compute(c, a.keySize, a.ivSize);
                d.iv = c.iv;
                a = g.encrypt.call(this, a, b, c.key, d);
                a.mixIn(c);
                return a;
            },
            decrypt: function(a, b, c, d) {
                d = this.cfg.extend(d);
                b = this._parse(b, d.format);
                c = d.kdf.compute(c, a.keySize, a.ivSize, b.salt);
                d.iv = c.iv;
                return g.decrypt.call(this, a, b, c.key, d);
            }
        });
}();