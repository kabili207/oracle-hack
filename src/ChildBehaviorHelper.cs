using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Various helper functions for use with the child's behavior
	/// </summary>
	public static class ChildBehaviorHelper
	{
		/// <summary>
		/// Gets the value of the child's behavior based on reponses to questions asked
		/// </summary>
		/// <param name="region">The region of the game</param>
		/// <param name="name">The name of the child</param>
		/// <returns></returns>
		public static byte GetValue(GameRegion region, string name)
		{
			Encoding encoding = region.GetEncoding();
			byte[] bytes = encoding.GetBytes(name);
			int value = 0;

			foreach (byte a in bytes)
			{
				value += ( a & 0xF );
			}

			do { value -= 3; } while (value > -1);
			return (byte)( value + 4 );
		}


		/// <summary>
		/// Gets the value of the child's behavior based on reponses to questions asked
		/// </summary>
		/// <param name="region">The region of the game</param>
		/// <param name="name">The name of the child</param>
		/// <param name="rupeesGiven">The number of rupees given</param>
		/// <param name="sleepMethod">The method used to help the child sleep</param>
		/// <returns></returns>
		public static byte GetValue(GameRegion region, string name, RupeesGiven rupeesGiven, SleepMethod sleepMethod)
		{
			return (byte)( GetValue(region, name) + (int)rupeesGiven + (int)sleepMethod );
		}

		/// <summary>
		/// Gets the value of the child's behavior based on reponses to questions asked
		/// </summary>
		/// <param name="region">The region of the game</param>
		/// <param name="name">The name of the child</param>
		/// <param name="rupeesGiven">The number of rupees given</param>
		/// <param name="sleepMethod">The method used to help the child sleep</param>
		/// <param name="childsQuestion">The response to the child's question</param>
		/// <param name="kindOfChild">Kind of child the player was</param>
		/// <returns></returns>
		public static byte GetValue(GameRegion region, string name, RupeesGiven rupeesGiven, SleepMethod sleepMethod, ChildQuestion childsQuestion, KindOfChild kindOfChild)
		{
			return (byte)( GetValue(region, name, rupeesGiven, sleepMethod) + (int)childsQuestion + (int)kindOfChild );
		}

		/// <summary>
		/// Gets the child's behavior based on the byte value supplied
		/// </summary>
		/// <param name="value">The raw behavior value</param>
		/// <returns></returns>
		public static ChildBehavior GetBehavior(byte value)
		{
			if (value == 0) return ChildBehavior.None;
			if (value < 6) return ChildBehavior.Curious;
			if (value < 11) return ChildBehavior.Shy;
			return ChildBehavior.Hyperactive;
		}
	}
}
