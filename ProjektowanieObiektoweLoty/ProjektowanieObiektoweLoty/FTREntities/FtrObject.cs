using NetTopologySuite.Noding;
using NetworkSourceSimulator;
using ProjektowanieObiektoweLoty.FQL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public interface IFtr
    {
        public virtual void AddToList() { }
        public virtual void AddToListFTR() { }
        public virtual void updateOnIDEvent(IDUpdateArgs args) { }
        public virtual void updateOnPositionEvent(PositionUpdateArgs args) { }
        public virtual void updateOnContactEvent(ContactInfoUpdateArgs args) { }
        public virtual bool EvaluateFQL(string cond) {  return false; }
        public virtual string GetField(string field)  { return null; }
        public virtual string[] GetAllFields() { return null;}
        public virtual IFtr Cast() { return null; }
        public virtual void SetField(string value, string field) { }
        public virtual string[] GetFieldNames() { return null; }
    }
    public abstract class FtrObject : IFtr
    {
        public string ClassName;
        public UInt64 ID;
        public FtrObject() { }
        public FtrObject(string className, UInt64 iD)
        {
            ClassName = className;
            ID = iD;
            Program.eventManager.IDEventSubscribe(this);
        }
        public virtual void AddToList() { }
        public virtual void AddToListFTR() { }
        public virtual void updateOnIDEvent(IDUpdateArgs args) { }
        public virtual void updateOnPositionEvent(PositionUpdateArgs args) { }
        public virtual void updateOnContactEvent(ContactInfoUpdateArgs args) { }
        public virtual bool EvaluateFQL(string cond) { return false; }
        public virtual string GetField(string field)  { return null; }
        public virtual string[] GetAllFields() { return null; }
        public virtual IFtr Cast() { return null; }
        public virtual void SetField(string value, string field) { }
        public virtual string[] GetFieldNames() { return null; }

    }
    public interface IFTRCreator
    {
        public FtrObject Create(string[] ObjectParameters);
        public FtrObject Create(byte[] ObjectParameters);
        public FtrObject Create();
        public void TestIfAllArguments(int FieldCount, int ArgumentsFromFileCount);
    }

    public abstract class CreatorFTR : IFTRCreator
    {
        public abstract FtrObject Create(string[] ObjectParameters);
        public abstract FtrObject Create(byte[] ObjectParameters);
        public void TestIfAllArguments(int FieldCount, int ArgumentsFromFileCount)
        {
            if (FieldCount != ArgumentsFromFileCount)
            {
                throw new ArgumentException("invalid number of arguments");
            }
        }
        public abstract FtrObject Create();
    }
    public struct WorldPositionC : IComparable
    {
        public double Latitude;

        public double Longitude;
        public WorldPositionC(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public int CompareTo(object? obj)
        {
            WorldPositionC other = (WorldPositionC)obj;
            double distanceThis = Math.Sqrt(Math.Pow(Latitude, 2) + Math.Pow(Longitude, 2));
            double distanceOther = Math.Sqrt(Math.Pow(other.Latitude, 2) + Math.Pow(other.Longitude, 2));
            return distanceThis.CompareTo(distanceOther);
        }
        public static WorldPositionC operator +(WorldPositionC left, WorldPositionC right)
        {
            return new WorldPositionC(left.Latitude + right.Latitude, left.Longitude + right.Longitude);
        }
        public static WorldPositionC operator *(WorldPositionC pos, double scalar)
        {
            return new WorldPositionC(pos.Latitude * scalar, pos.Longitude * scalar);
        }
    }

}
