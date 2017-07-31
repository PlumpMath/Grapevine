using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Grapevine
{
    public class Hash
    {
        public Hash(byte[] digest) => Digest = digest;
        public byte[] Digest { get; private set; }

        public static implicit operator Hash(byte[] digest) => new Hash(digest);
        public static explicit operator byte[] (Hash hash) => hash.Digest;
        public override string ToString() => string.Join("", Digest.Select(b => b.ToString("x2")));
        
        public BigInteger ToBigInteger()
        {            
            var bytes = Digest;            
            // Little Endian for Big Integers
            Array.Reverse(bytes);

            // Appending 0x00 to the end forces it to become unsigned
            Array.Resize(ref bytes, bytes.Length + 1);

            return new BigInteger(bytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;

            var other = (Hash)obj;
            return Digest.SequenceEqual(other.Digest);
        }

        public override int GetHashCode()
        {
            return Digest.GetHashCode();
        }
    }
}
