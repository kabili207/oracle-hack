/*
 *  Copyright © 2013-2018, Amy Nagle.
 *  All rights reserved.
 *
 *  This file is part of ZoraSharp.
 *
 *  ZoraSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  ZoraSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with ZoraSharp. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;

#if !BLAZOR
using System.ComponentModel;
#endif

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents the user data for an individual game
	/// </summary>
#if !BLAZOR
	[Serializable]
#endif
	public class GameInfo
#if !BLAZOR
		: INotifyPropertyChanged
#endif
	{

		#region Fields

		private GameRegion region = GameRegion.US;
		private string hero = "\0\0\0\0\0";
		private string child = "\0\0\0\0\0";
		private short gameId = 0;
		private byte behavior = 0;
		private byte animal = 0;
		private Game game = 0;
		private bool isHeroQuest = false;
		private bool isLinkedGame = false;
		private long rings = 0L;
		private bool wasGivenFreeRing = false;

		#endregion // Fields

#if !BLAZOR

		/// <summary>
		/// Occurs when a property has changed
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

#endif

		#region Properties

		/// <summary>
		/// Gets or sets the Region used for this user data
		/// </summary>
		public GameRegion Region
		{
			get => region;
			set => SetProperty(ref region, value, nameof(Region));
		}

		/// <summary>
		/// Gets or sets the Game used for this user data
		/// </summary>
		public Game Game
		{
			get => game;
			set => SetProperty(ref game, value, nameof(Game));
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsHeroQuest
		{
			get => isHeroQuest;
			set => SetProperty(ref isHeroQuest, value, nameof(IsHeroQuest));
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsLinkedGame
		{
			get => isLinkedGame;
			set => SetProperty(ref isLinkedGame, value, nameof(IsLinkedGame));
		}

		/// <summary>
		/// Gets or sets the unique game ID
		/// </summary>
		public short GameID
		{
			get => gameId;
			set => SetProperty(ref gameId, value, nameof(GameID));
		}

		/// <summary>
		/// Gets or sets the hero's name
		/// </summary>
		public string Hero
		{
			get => hero.Trim(' ', '\0');
			set => SetProperty(ref hero, value.NullPad(5), nameof(Hero));
		}

		/// <summary>
		/// Gets or sets the child's name
		/// </summary>
		public string Child
		{
			get => child.Trim(' ', '\0');
			set => SetProperty(ref child, value.NullPad(5), nameof(Child));
		}

		/// <summary>
		/// Gets or sets the animal friend
		/// </summary>
		public Animal Animal
		{
			get => (Animal)animal;
			set {
				if (( value < 0 ) || ( (long)value > byte.MaxValue ))
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				SetProperty(ref animal, (byte)value, nameof(Animal));
			}
		}

		/// <summary>
		/// Gets or set the behavior of the child
		/// </summary>
		public byte Behavior
		{
			get => behavior;
			set => SetProperty(ref behavior, value, nameof(Behavior));
		}

		/// <summary>
		/// Gets or sets the value indicating if Vasu has given the player a free ring
		/// </summary>
		public bool WasGivenFreeRing
		{
			get => wasGivenFreeRing;
			set => SetProperty(ref wasGivenFreeRing, value, nameof(WasGivenFreeRing));
		}

		/// <summary>
		/// Gets or sets the user's ring collection
		/// </summary>
		public Rings Rings
		{
			get => (Rings)rings;
			set => SetProperty(ref rings, (long)value, nameof(Rings));
		}

		#endregion // Properties

		/// <summary>
		/// Compares a field's current value against a new value.
		/// If they are different, sets the field to the new value and
		/// sends a notification that the property has changed.
		/// </summary>
		/// <param name="field">Reference to the field storing current value</param>
		/// <param name="value">New value to ensure the field has</param>
		/// <param name="name">Name of the property being changed</param>
		/// <example>
		/// <code language="C#">
		/// private short _gameID = 0;
		/// public short GameID
		/// {
		///     get { return _gameID; }
		///     set
		///     {
		///         SetProperty(ref _gameID, value, "GameID");
		///     }
		/// }
		/// </code>
		/// </example>
		private void SetProperty<T>(ref T field, T value, string name)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
#if !BLAZOR
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
#endif
			}
		}

		#region File Saving/Loading methods

		/// <summary>
		/// Writes this game info out to the specified file
		/// </summary>
		/// <param name="filename">The file name</param>
		/// <example>
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// GameInfo info = new GameInfo();
		/// info.Write(file);
		/// </code>
		/// </example>
		public void Write(string filename)
		{
			using (FileStream outFile = File.Create(filename))
			{
				Write(outFile);
			}
		}

		/// <summary>
		/// Writes the game info to the specified stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <example>
		/// This example demonstrates creating a .zora save file from a<see cref="GameInfo"/>
		/// object. If you have access to the file name you will be saving to, as in this
		/// example, you should consider using the <see cref="Write(string)"/> method instead.
		/// <code language="C#">
		/// GameInfo info = new GameInfo();
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// using (FileStream outFile = File.Create(file))
		/// {
		///     info.Write(fileStream);
		/// }
		/// </code>
		/// </example>
		public void Write(Stream stream)
		{
			using (var swriter = new StreamWriter(stream))
			{
				var converter = new GameInfoJsonConverter();
				IDictionary<string, object> dict = converter.Serialize(this);
				string json = SimpleJson.SimpleJson.SerializeObject(dict);
				swriter.WriteLine(json);
			}
		}

		/// <summary>
		/// Loads the game info from the specified file
		/// </summary>
		/// <param name="filename">The file name of the saved GameInfo</param>
		/// <returns>A GameInfo</returns>
		/// <example>
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// GameInfo info = GameInfo.Load(file);
		/// </code>
		/// </example>
		public static GameInfo Load(string filename)
		{
			using (FileStream inFile = File.OpenRead(filename))
			{
				return Load(inFile);
			}
		}

		/// <summary>
		/// Loads the game from the specified stream
		/// </summary>
		/// <param name="stream">The stream containing the saved GameInfo</param>
		/// <returns>A GameInfo</returns>
		/// <example>
		/// This example demonstrates creating a <see cref="GameInfo"/> object from a .zora
		/// save file. If you have access to the file name you will be loading from, as in
		/// this example, you should consider using the <see cref="Load(string)"/> method instead.
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// using (FileStream fileStream = File.OpenRead(file))
		/// {
		///     GameInfo info = GameInfo.Load(fileStream);
		/// }
		/// </code>
		/// </example>
		public static GameInfo Load(Stream stream)
		{
			using (var sreader = new StreamReader(stream))
			{
				string json = sreader.ReadToEnd();
				return Parse(json);
			}
		}

		/// <summary>
		/// Parses the specified json.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>A game info object</returns>
		/// <example>
		/// <code language="C#">
		/// string json = @"
		/// {
		///    ""Region"": ""US"",
		///    ""Game"": ""Ages"",
		///    ""GameID"": 14129,
		///    ""Hero"": ""Link"",
		///    ""Child"": ""Pip"",
		///    ""Animal"": ""Dimitri"",
		///    ""Behavior"": ""BouncyD"",
		///    ""IsLinkedGame"": true,
		///    ""IsHeroQuest"": false,
		///    ""Rings"": -9222246136947933182
		/// }";
		/// GameInfo info = GameInfo.Parse(json);
		/// </code>
		/// </example>
		public static GameInfo Parse(string json)
		{
			var dict = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(json);
			var converter = new GameInfoJsonConverter();
			return converter.Deserialize(dict);
		}

		#endregion // File Saving/Loading methods

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (GetType() != obj?.GetType())
			{
				return false;
			}

			var g = (GameInfo)obj;

			return
				( region == g.region ) &&
				( gameId == g.gameId ) &&
				( game == g.game ) &&
				( hero == g.hero ) &&
				( child == g.child ) &&
				( behavior == g.behavior ) &&
				( animal == g.animal ) &&
				( isHeroQuest == g.isHeroQuest ) &&
				( isLinkedGame == g.isLinkedGame ) &&
				( rings == g.rings ) &&
				( wasGivenFreeRing == g.wasGivenFreeRing );

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return region.GetHashCode() ^
				gameId.GetHashCode() ^
				game.GetHashCode() ^
				hero.GetHashCode() ^
				child.GetHashCode() ^
				behavior.GetHashCode() ^
				animal.GetHashCode() ^
				isHeroQuest.GetHashCode() ^
				isLinkedGame.GetHashCode() ^
				rings.GetHashCode() ^
				wasGivenFreeRing.GetHashCode();
		}
	}
}
