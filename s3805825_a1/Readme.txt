Design Pattern Used: Facade

Facade role: The client can call its methods, and the Facade role can know the functions and responsibilities of the relevant (one or more) subsystems;
Under normal circumstances, it delegates all requests from the client to the corresponding subsystem and passes them to the corresponding subsystem object for processing.

SubSystem role: we can have one or more SubSystem roles in the software system, each SubSystem can not be a single class, but a collection of classes,
it realizes the function of the SubSystem; Each subsystem can be called either directly by the client or by the facade role,
which handles requests from the facade class; The subsystem is unaware of the appearance, and to the subsystem, the appearance role is just another client.

I put the deposit, withdraw and transfer method and their validation inside of the Class Customer.
It not only decrease the complexity of the menu or run class, it's easier for client to call that method.

Design Pattern Used: Builder

It separates the construction of a complex object from its representation so that the same construction process can create different representations.
The Builder pattern separates the build code from the presentation code,
so that the client does not have to know the details of the internal composition of the product,
thus reducing the coupling between the client and the specific product.

In my code, Getting transactions for the Account, Getting Accounts for Customers are sperated. This increase the complexity and security for clients to visit the code because the relation between each method and classes are complex.

Explaination for Using Class Library
It's easy to manage classes and it's easy for people to call classes, If we throw all the classes together, it's just a mess and we can't figure out what these class do.
You can also run into problems with class names
Classes that accomplish certain goals are "packaged" together to make it easier to manage.

