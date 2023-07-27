﻿using me.cqp.luohuaming.Dice.Sdk.Cqp.Expand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.luohuaming.Dice.Sdk.Cqp.Model
{
	/// <summary>
	/// 描述 QQ好友信息列表 的类
	/// </summary>
	public class FriendInfoCollection : BasisStreamModel, IReadOnlyCollection<FriendInfo>, IEquatable<FriendInfoCollection>
	{
		#region --字段--
		private List<FriendInfo> _list;
		#endregion

		#region --属性--
		/// <summary>
		/// 获取 <see cref="FriendInfoCollection"/> 中包含的元素数
		/// </summary>
		public int Count
		{
			get { return this._list.Count; }
		}
		#endregion

		#region --构造函数--
		/// <summary>
		/// 使用指定的密文初始化 <see cref="FriendInfoCollection"/> 类的新实例
		/// </summary>
		/// <param name="api">模型使用的 <see cref="Cqp.CQApi"/></param>
		/// <param name="cipherText">模型使用的解密密文字符串</param>
		public FriendInfoCollection (CQApi api, string cipherText)
			: base (api, cipherText)
		{ }
		#endregion

		#region --公开方法--
		/// <summary>
		/// 返回一个循环访问集合的枚举器
		/// </summary>
		/// <returns>用于循环访问集合的枚举数</returns>
		public IEnumerator<FriendInfo> GetEnumerator ()
		{
			return this._list.GetEnumerator ();
		}
		/// <summary>
		/// 返回一个循环访问集合的枚举器
		/// </summary>
		/// <returns>用于循环访问集合的枚举数</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}
		/// <summary>
		/// 指示当前对象是否等于同一类型的另一个对象
		/// </summary>
		/// <param name="other">一个与此对象进行比较的对象</param>
		/// <returns>如果当前对象等于 other 参数，则为 <see langword="true"/>；否则为 <see langword="false"/></returns>
		public bool Equals (FriendInfoCollection other)
		{
			if (other == null)
			{
				return false;
			}

			return this._list.SequenceEqual (other._list);
		}
		/// <summary>
		/// 指示当前对象是否等于同一类型的另一个对象
		/// </summary>
		/// <param name="obj">一个与此对象进行比较的对象</param>
		/// <returns>如果当前对象等于 other 参数，则为 <see langword="true"/>；否则为 <see langword="false"/></returns>
		public override bool Equals (object obj)
		{
			return this.Equals (obj as FriendInfoCollection);
		}
		/// <summary>
		/// 返回此实例的哈希代码
		/// </summary>
		/// <returns>32 位有符号整数哈希代码</returns>
		public override int GetHashCode ()
		{
			return this._list.GetHashCode ();
		}
		/// <summary>
		/// 返回表示当前对象的字符串
		/// </summary>
		/// <returns>表示当前对象的字符串</returns>
		public override string ToString ()
		{
			StringBuilder builder = new StringBuilder ();
			foreach (FriendInfo item in this)
			{
				builder.AppendFormat ("QQ: {0}{1}", item.QQ.Id, Environment.NewLine);
				builder.AppendFormat ("\t昵称: {0}{1}", item.Nick, Environment.NewLine);
				builder.AppendFormat ("\t备注: {0}{1}", item.Postscript, Environment.NewLine);
				builder.AppendLine ();
			}
			return builder.ToString ();
		}
		/// <summary>
		/// 返回用于发送的字符串
		/// </summary>
		/// <returns>用于发送的字符串</returns>
		public override string ToSendString ()
		{
			return this.ToString ();
		}
		#endregion

		#region --私有方法--
		/// <summary>
		/// 当在派生类中重写时, 进行当前模型初始化
		/// </summary>
		/// <param name="reader">解析模型的数据源</param>
		protected override void Initialize (BinaryReader reader)
		{
			if (this._list == null)
			{
				this._list = new List<FriendInfo> ();
			}

			int count = reader.ReadInt32_Ex ();
			for (int i = 0; i < count; i++)
			{
				if (reader.Length () <= 0)
				{
					throw new EndOfStreamException ("无法读取数据, 因为已经读取到数据流末尾");
				}

				this._list.Add (new FriendInfo (this.CQApi, reader.ReadToken_Ex ()));
			}
		}
		#endregion
	}
}
