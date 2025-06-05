using Server.Extensions;
using System.Buffers;
using System.Text;

namespace Server.Models.PushMessages
{
	public class PushMessage
	{
		public string Message { get; set; } = string.Empty;

		public ReadOnlySequence<byte> GetMessageBytes()
		{
			return Message.GetByteSequence();
		}
	}
}
