namespace OOP_Assignment_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Q1
            // Q1 Object Copying
            // a) What happens when you assign one object variable to another object variable?
            // both variables refer to the same object in memory.

            // b) Does assigning one object to another create a new object? Explain.
            // No,It only copies Make botth of them refer to the same objet.

            // c) What is the difference between copying an object and copying its reference?
            // Copying an object creates a new object with the same values, while copying a reference only creates another reference to the same object.
            #endregion

            #region Q2
            // Shallow Copy vs Deep Copy
            //a) What is a Shallow Copy?
            // it only copies the values of the original object. If the original object has reference-type fields, the shallow copy will still refer to the same objects in memory as the original object.

            //b) What is a Deep Copy?
            // creatiing a new object that is a copy of the original object.

            //c) What happens to reference-type members when a Shallow Copy is created?
            // They are not copied and will refer to the same objects in memory as the original object.

            //d) What happens to reference-type members when a Deep Copy is created?
            // Reference-type members are copied too, creating new instances of the referenced objects.

            //e) Give one situation where Deep Copy would be safer than Shallow Copy.
            // when you want to ensure that modifications to the copied object do not affect the original object or any other objects that reference it.
            #endregion

            #region Q3
            // Static Members
            //a) What is a static field, and how is it different from an instance field ?
            // A static is a variable that is shared among the class, instance field is unique.
            // Static fields are associated with the class itself.

            //b) What is a static method? Can a static method directly access instance members?
            // A static method belongs to the class not object in the class.
            // It can be called without creating an object of the class.
            // cannot directly access instance members because it does not have a reference to a specific instance of the class.

            //c) What is a static constructor, and when is it executed ?
            // A static constructor is used to initialize static members of a class.
            // It is executed only once, when the class is first accessed
            
            //d) What is a static class? Can you create an object from a static class?
            // A static class is a class that cannot be instantiated.
            // You cannot create an object from a static class.
            #endregion
        }
    }
}
