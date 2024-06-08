using CoreLibrary.Models;

namespace CoreLibrary.Helpers
{
    public static class Helpers
    {

        public static int SelectOrInsert<T>(T model) where T : CoreDbModel
        {
            var db = new BaseRepository();
            var selectedObj = db.GetSelectAllExceptKey(model);
            int id;
            if (selectedObj == null)
                id = db.Insert(model);
            else
            {
                var idProperty = selectedObj.GetType().GetProperty("Id");
                id = (int)idProperty.GetValue(selectedObj)!;
            }
            return id;
        }
    }
}
