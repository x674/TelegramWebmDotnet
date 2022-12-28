using System.ComponentModel.DataAnnotations;

public class Catalog
{
    public int current_thread { get; set; }
    public int files_count { get; set; }
    public bool is_board { get; set; }
    public int is_closed { get; set; }
    public bool is_index { get; set; }
    public int max_num { get; set; }
    public int posts_count { get; set; }
    public string thread_first_image { get; set; }
    public Threads[] threads { get; set; }
    public string title { get; set; }
    public int unique_posters { get; set; }
}

public class Threads
{
    public Posts[] posts { get; set; }
    
    public int closed { get; set; }
    public string date { get; set; }
    public string email { get; set; }
    public int endless { get; set; }
    public Files[] files { get; set; }
    public int files_count { get; set; }
    public int lasthit { get; set; }
    public string name { get; set; }
    public Threads[] threads { get; set; }
    public int num { get; set; }
    public int op { get; set; }
    public int parent { get; set; }
    public int posts_count { get; set; }
    public int sticky { get; set; }
    public string subject { get; set; }
    public string tags { get; set; }
    public int timestamp { get; set; }
    public string trip { get; set; }
    public int views { get; set; }
}

public class Files
{
    public string fullname { get; set; }
    public int height { get; set; }
    [Key]
    public string md5 { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public int size { get; set; }
    public string thumbnail { get; set; }
    public int type { get; set; }
    public int width { get; set; }
    public int duration_secs { get; set; }
}

public class Posts
{
    public int banned { get; set; }
    public string board { get; set; }
    public int closed { get; set; }
    public string comment { get; set; }
    public string date { get; set; }
    public string email { get; set; }
    public int endless { get; set; }
    public Files[] files { get; set; }
    public int lasthit { get; set; }
    public string name { get; set; }
    public int num { get; set; }
    public int number { get; set; }
    public int op { get; set; }
    public int parent { get; set; }
    public int sticky { get; set; }
    public string subject { get; set; }
    public string tags { get; set; }
    public int timestamp { get; set; }
    public string trip { get; set; }
    public int views { get; set; }
}


