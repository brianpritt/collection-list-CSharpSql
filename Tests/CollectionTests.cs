using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory.Objects
{
  public class CollectionTest : IDisposable
  {
    public void InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Collection.GetAll().Count;
      Assert.Equal(0,result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Collection newItem = new Collection("Palm Pilot");
      Collection secondItem = new Collection("Palm Pilot");

      Assert.Equal(newItem, secondItem);
    }

    [Fact]
    public void Test_Equal_SavesToDatabase()
    {
      //Arrange
      Collection newItem = new Collection("Palm Pilot");

      //Act
      newItem.Save();
      List<Collection> result = Collection.GetAll();
      List<Collection> items = new List<Collection>{newItem};

      //Assert
      Assert.Equal(result, items);
    }
    [Fact]
    public void Test_Saves_AssignsIdObject()
    {
      //arrange
      Collection testCollection = new Collection("Palm Pilot");
      //Act
      testCollection.Save();
      Collection savedCollection = Collection.GetAll()[0];

      int result = savedCollection.GetId();
      int testId = testCollection.GetId();

      Assert.Equal(result,testId);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Collection testCollection = new Collection("Palm Pilot");
      testCollection.Save();
      //Act
      Collection foundCollection = Collection.Find(testCollection.GetId());
      //Assert
      Assert.Equal(testCollection, foundCollection);
    }

    public void Dispose()
    {
      Collection.DeleteAll();
    }
  }
}
