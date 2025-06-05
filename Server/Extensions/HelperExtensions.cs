using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server.Extensions
{
	public static class HelperExtensions
	{
		public static ReadOnlySequence<byte> GetByteSequence(this string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			return new ReadOnlySequence<byte>(bytes);
		}
	}
}
