using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Inventory.Objects
{
	public class Collection
	{
		private string _name;
		private int _id;

		public Collection(string name, int id = 0)
		{
			_name = name;
			_id = id;
		}
		public override bool Equals(Object otherItem)
		{
			if (!(otherItem is Collection))
			{
				return false;
			}
			else
			{
				Collection newCollection = (Collection) otherItem;
				bool idEquality = (this.GetId() == newCollection.GetId());
				bool nameEquality = (this.GetName() == newCollection.GetName());
				return (idEquality && nameEquality);
			}
		}
		public int GetId()
		{
			return _id;
		}
		public string GetName()
		{
			return _name;
		}

		public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("INSERT INTO items (name) OUTPUT INSERTED.id VALUES (@collectionName);", conn);

			SqlParameter nameParameter = new SqlParameter();
			nameParameter.ParameterName = "@collectionName";
			nameParameter.Value = this._name;
			cmd.Parameters.Add(nameParameter);
			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
				this._id = rdr.GetInt32(0);
			}
			if (rdr != null)
			{
				rdr.Close();
			}
			if (conn != null)
			{
				conn.Close();
			}
		}

		public static List<Collection> GetAll()
		{
			List<Collection> allCollections = new List<Collection> {};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM items;", conn);
			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
				string collectionName = rdr.GetString(0);
				int collectionId = rdr.GetInt32(1);
				Collection newCollection = new Collection(collectionName, collectionId);
				allCollections.Add(newCollection);
			}

			if (rdr != null)
			{
				rdr.Close();
			}
			if (conn != null)
			{
				conn.Close();
			}
			return allCollections;
		}

		public static Collection Find(int id)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM items WHERE id = @collectionId;", conn);
			SqlParameter idParameter = new SqlParameter();
			idParameter.ParameterName = "@collectionId";
			idParameter.Value = id.ToString();
			cmd.Parameters.Add(idParameter);
			SqlDataReader rdr = cmd.ExecuteReader();

			int foundCollectionId = 0;
			string foundCollectionName = "";
			while (rdr.Read())
			{
				foundCollectionName = rdr.GetString(0);
				foundCollectionId = rdr.GetInt32(1);
			}
			Collection foundCollection = new Collection(foundCollectionName, foundCollectionId);

			if (rdr != null)
			{
				rdr.Close();
			}
			if (conn != null)
			{
				conn.Close();
			}

			return foundCollection;
		}

		public static void DeleteAll()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("DELETE FROM items;", conn);
			cmd.ExecuteNonQuery();

			conn.Close();
		}
	}
}
