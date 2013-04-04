﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinForms
{
    public class FakeStudioController: HuntingDog.DogEngine.IStudioController
    {


        public event Action<List<string>> OnServersAdded;

        public event Action<List<string>> OnServersRemoved;

        public List<HuntingDog.DogEngine.Entity> Find(string serverName, string databaseName, string searchText)
        {
            if (searchText == "slow")
                System.Threading.Thread.Sleep(6 * 1000);

            if (EmptyDataBase == databaseName)
                return new List<HuntingDog.DogEngine.Entity>();

            if (OnAction != null)
                OnAction("Find:" + searchText + " on " + databaseName + ", srv:" + serverName);
            searchText = searchText.ToLower();

            IEnumerable<HuntingDog.DogEngine.Entity> storage = null;
            if (databaseName == SmallDataBase)
            {
                storage = FakeFindList.Take(10);
            }
            else
                storage = FakeFindList;


            if (searchText == " " || searchText == "")
                return FakeFindList;

           
            return FakeFindList.Where(x=>x.Name.ToLower().Contains(searchText)).OrderBy(x=>x.Name).ToList();
        }

        public void Initialise()
        {
            if (OnAction != null)
                OnAction("Initialise");
        }

        public void FireOnServersChanged()
        {
          
        }

        int dupNumber = 0;
        public void DuplicateFakeList()
        {
            dupNumber++;
            foreach (var e in FakeFindList.ToList())
            {
                if (e.FullName == null)
                    e.FullName = "dbo." + e.Name;

                var newE = new HuntingDog.DogEngine.Entity();
                newE.IsFunction = e.IsFunction;
                newE.IsTable = e.IsTable;
                newE.IsView = e.IsView;
                newE.IsProcedure = e.IsProcedure;
             
             
             
                    newE.Name = e.Name + dupNumber;

              newE.FullName = "dbo." + newE.Name;
                FakeFindList.Add(newE);
            }
        }

        public string EmptyDataBase { get; set; }
        public string SmallDataBase { get; set; }

        public event Action<string> OnAction;
        public List<HuntingDog.DogEngine.Entity> FakeFindList { get; set; }

        public List<HuntingDog.DogEngine.Entity> AlternativeFakeFindList { get; set; }

        public List<HuntingDog.DogEngine.Entity> FakeInvokedByList { get; set; }

        public List<HuntingDog.DogEngine.Entity> FakeInvokesList { get; set; }


        public List<string> FakeServers { get; set; }
        public List<string> FakeDatabases { get; set; }

        public List<string> ListServers()
        {
            return FakeServers;
        }

        public List<string> ListDatabase(string serverName)
        {
            return FakeDatabases;
        }

        public void RefreshServer(string serverName)
        {
            if (OnAction != null)
                OnAction("Refresh server: " + serverName);
        }

        public void RefreshDatabase(string serverName, string databaseName)
        {
            if (OnAction != null)
                OnAction("Refresh database: " + serverName + ", db:" + databaseName);
        }

        public List<HuntingDog.DogEngine.TableColumn> ListColumns(HuntingDog.DogEngine.Entity entityObject)
        {
            var res = new List<HuntingDog.DogEngine.TableColumn>();
            string name = entityObject.Name;

            for (int i = 0; i < name.Length / 2; i++)
            {
                var clm = new HuntingDog.DogEngine.TableColumn();
                clm.IsPrimaryKey = i == 0?true:false;
                clm.IsForeignKey = (i % 3 == 0) && (i!=0);

                if (i == 0)
                    clm.Name = name + "Id";
                else
                    clm.Name = "Column" + i.ToString();

                clm.Nullable = (i % 2) != 0;
                if (i == 0)
                    clm.Type = "int";
                else 
                    clm.Type = (i%3==0) ? "varchar(max)":"decimal";

                 res.Add(clm);

            }

          
               

            return res;
        }


        public List<HuntingDog.DogEngine.FunctionParameter> ListFuncParameters(HuntingDog.DogEngine.Entity entityObject)
        {
            return new List<HuntingDog.DogEngine.FunctionParameter>();
        }

        public List<HuntingDog.DogEngine.ProcedureParameter> ListProcParameters(HuntingDog.DogEngine.Entity entityObject)
        {
            string name = entityObject.Name;

            var res = new List<HuntingDog.DogEngine.ProcedureParameter>();
            for (int i = 0; i < name.Length / 2; i++)
            {
                var clm = new HuntingDog.DogEngine.ProcedureParameter();
                if (i == 0)
                    clm.Name = name + "Id";
                else
                    clm.Name = "Parameter" + i;


                if (i == 0)
                    clm.Type = "int";
                else
                    clm.Type = (i % 3 == 0) ? "varchar(max)" : "decimal";

                clm.DefaultValue = (i % 2) != 0 ? "NULL":"";

                res.Add(clm);

            }

            if(name.Length % 3 == 0)
                res[res.Count - 1].IsOut = true;

            return res;

        }


        public List<HuntingDog.DogEngine.TableColumn> ListViewColumns(HuntingDog.DogEngine.Entity entityObject)
        {
            return ListColumns(entityObject);
        }

      


        public List<HuntingDog.DogEngine.Entity> GetInvokedBy(HuntingDog.DogEngine.Entity entityObject)
        {
            return FakeInvokedByList;
        }

        public List<HuntingDog.DogEngine.Entity> GetInvokes(HuntingDog.DogEngine.Entity entityObject)
        {
            return FakeInvokesList ;
        }



        public void ModifyFunction(string name)
        {
            if (OnAction != null)
                OnAction("Modify function: " + name);
        }

        public void ModifyView(string name)
        {
            if (OnAction != null)
                OnAction("Modify view: " + name);
        }

        public void ModifyProcedure(string name)
        {
            if (OnAction != null)
                OnAction("Modify proc: " + name);
        }

        public void SelectFromTable(string name)
        {
            if (OnAction != null)
                OnAction("Select from table: " + name);
        }

        public void SelectFromView(string name)
        {
            if (OnAction != null)
                OnAction("Select from view: " + name);
        }

        public void ExecuteProcedure(string name)
        {
            if (OnAction != null)
                OnAction("execute stored proc: " + name);
        }

        public void ExecuteFunction(string name)
        {
            if (OnAction != null)
                OnAction("execute function: " + name);
        }

        public void EditTableData(string name)
        {
            if (OnAction != null)
                OnAction("edit table: " + name);
        }

        public void DesignTable(string name)
        {
            if (OnAction != null)
                OnAction("design table: " + name);
        }

        public void GenerateCreateScript(string name)
        {
            if (OnAction != null)
                OnAction("generate Create script: " + name);
        }

        public void NavigateObject(string server, HuntingDog.DogEngine.Entity name)
        {
            if (OnAction != null)
                OnAction("Navigate Object: " + name.FullName);
        }


        public void ModifyFunction(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("Modify func: " + entityObject.FullName);
        }

        public void ModifyView(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("Modify view: " + entityObject.FullName);
        }

        public void ModifyProcedure(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("Modify proc: " + entityObject.FullName);
        }

        public void SelectFromTable(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("select table: " + entityObject.FullName);
        }

        public void SelectFromView(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("select view: " + entityObject.FullName);
        }

        public void ExecuteProcedure(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("exec proc: " + entityObject.FullName);
        }

        public void ExecuteFunction(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("exec func: " + entityObject.FullName);
        }

        public void EditTableData(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("edit table: " + entityObject.FullName);
        }

        public void DesignTable(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            if (OnAction != null)
                OnAction("design table: " + entityObject.FullName);
        }

        public event Action ShowYourself;

        public void ConnectNewServer()
        {
            if (OnAction != null)
                OnAction("Connect New ");
        }




        public void ScriptTable(string server, HuntingDog.DogEngine.Entity entityObject)
        {
            throw new NotImplementedException();
        }



    }
}
