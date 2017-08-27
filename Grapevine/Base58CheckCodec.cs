using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine
{
    public class Base58CheckCodec : BaseCodec
    {
        public Base58CheckCodec()
            : base("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz")
        { }

        public override string Encode(byte[] data)
        {
            return new string(Alphabet[0], data.TakeWhile(b => b == 0).Count()) + base.Encode(data);
        }

        public override byte[] Decode(string encoded)
        {
            return Enumerable.Repeat<byte>(0, encoded.TakeWhile(c => c == '1').Count()).Concat(base.Decode(encoded)).ToArray();
        }
    }
}
