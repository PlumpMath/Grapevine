using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Grapevine
{
    public class BaseCodec
    {
        public BaseCodec(char[] alphabet)
        {
            Alphabet = alphabet;
        }

        public BaseCodec(string alphabet)
            : this (alphabet.ToCharArray())
        { }

        public int Base => Alphabet.Length;
        public char[] Alphabet { get; private set; }

        public virtual string Encode(byte[] data)
        {
            var sum = data.Aggregate(BigInteger.Zero, (c, t) => c * 256 + t);
            var result = string.Empty;
            while (!sum.IsZero)
            {
                sum = BigInteger.DivRem(sum, Alphabet.Length, out var rem);
                result = Alphabet[(int)rem] + result;
            }
            return result;
        }

        public virtual byte[] Decode(string encoded)
        {
            var sum = BigInteger.Zero;
            for (var i = 0; i < encoded.Length; i++)
                sum += BigInteger.Pow(Alphabet.Length, i) * Array.IndexOf(Alphabet, encoded[encoded.Length - i - 1]);

            var data = new Stack<byte>();
            while (!sum.IsZero)
            {
                sum = BigInteger.DivRem(sum, 256, out var rem);
                data.Push((byte)rem);
            }

            return data.ToArray();
        }

        public static readonly BaseCodec Base10 = new BaseCodec("0123456789");
        public static readonly BaseCodec Base16 = new BaseCodec("0123456789abcdef");
    }
}
