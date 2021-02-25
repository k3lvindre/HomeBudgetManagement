using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;

//Although you could write a complete IDependencyResolver implementation from scratch, the interface is really designed to act as bridge between Web API and existing IoC containers.

//An IoC container is a software component that is responsible for managing dependencies. You register types with the container, and then use the container to create objects. The container automatically figures out the dependency relations.Many IoC containers also allow you to control things like object lifetime and scope.

//"IoC" stands for "inversion of control", which is a general pattern where a framework calls into application code.An IoC container constructs your objects for you, which "inverts" the usual flow of control.

//For this tutorial, we'll use Unity from Microsoft Patterns & Practices. (Other popular libraries include Castle Windsor, Spring.Net, Autofac, Ninject, and StructureMap.) 
public class UnityResolver : IDependencyResolver
{
    protected IUnityContainer container;

    public UnityResolver(IUnityContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        this.container = container;
    }

    public object GetService(Type serviceType)
    {
        try
        {
            return container.Resolve(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        try
        {
            return container.ResolveAll(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return new List<object>();
        }
    }

//Controllers are created per request.To manage object lifetimes, IDependencyResolver uses the concept of a scope.

//The dependency resolver attached to the HttpConfiguration object has global scope. When Web API creates a controller, it calls BeginScope.This method returns an IDependencyScope that represents a child scope.

//Web API then calls GetService on the child scope to create the controller.When request is complete, Web API calls Dispose on the child scope.
    public IDependencyScope BeginScope()
    {
        var child = container.CreateChildContainer();
        return new UnityResolver(child);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        container.Dispose();
    }
}