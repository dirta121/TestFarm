namespace TestFarm.SaveData
{
    public struct SavePlayerData
    {
        public int gold { get; }
        public SaveWarehouseData warehouse { get; }
        public SaveFieldsData fields { get; }
        public SavePlayerData(int gold, SaveWarehouseData warehouse= new SaveWarehouseData(), SaveFieldsData fields=new SaveFieldsData())
        {
            this.gold = gold;
            this.warehouse = warehouse;
            this.fields = fields;
        }
    }
    public struct SaveWarehouseItem
    {
        public string name { get; }
        public int count { get; }
        public SaveWarehouseItem(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }
    public struct SaveWarehouseData
    {
        public SaveWarehouseItem[] items { get; }
        public SaveWarehouseData(SaveWarehouseItem[] items)
        {
            this.items = items;
        }
    }
    public struct SaveFieldsData 
    {
        public SaveFieldItem[] items { get; }
        public SaveFieldsData(SaveFieldItem[] items) 
        {
            this.items = items;
        }
    }
    public struct SaveFieldItem 
    {
        public string name { get; }
        public int[] cell { get; }
        public float[] position { get; }
        public float[] rotation { get; }
        public SaveFieldItem(string name, int[] cell,float[] position,float[] rotation)
        {
            this.name = name;
            this.cell = cell;
            this.position = position;
            this.rotation = rotation;
        }
    }
}