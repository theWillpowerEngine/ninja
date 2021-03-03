# Introducing ninja

ninja is a programming languages that combines declarative data and state with imperative commands to produce a unique and interesting development experience.  ninja objects can have events, semi-permanent state and can be saved to disk and restored to their previous point of functionality if necessary.

Using a lisp-like paradigm that eschews the separation between code and data, ninja is uniquely suited to small, atomic processes like microservices, CI/CD steps, development tools and as an imperative tool to assemble more complex applications from multiple different sources, programming languages and even platforms.

This simple hello world like example shows what ninja is about:

    my-app:
        data:
            what-to-say: Hello Nurse!

        when: ready: print $data.what-to-say

It's a little more complex than it needs to be (we don't have to load what we print from a data field), but it aims to show an extremely-contrived example of how code and data sit side by side and interact.  Here's the absolutely easiest way to do it:

    say-hello {
        print "Hello Nurse!"
    }

Confused?  Don't worry, it will all make sense soon...

## A Quick Guide to ninja Syntax

The question which one naturally asks upon looking at the preceding examples is:  WTF?  Is it YAML, Some kind of terrible JSON, or a weird, C-like programming language?  Or maybe Python?  The answer, of course, is none-of-the-above...  it's ninja!

The syntax of ninja is extremely straighforward, there are a couple of different constructs which combine to create an object representation.  There are three core syntactic elements in ninja, and one special way you can decorate them, and that's pretty much it for the declarative stuff.  The objects are:

* Verbatim blocks or long strings -- most often used to contain imperative code, but not necessarily.  Bracketed with curly-braces.
* Objects, collections of fields
* Fields, which are just name/value pairs

In addition, Fields and objects can have metavalues, which are little extra bits of metadata which help disambiguate fields and also provide details about particular types of field.  Lets dig into some examples:

Here is a simple object, consisting only of simple fields:

    person:
        name: Dan
        age:  37
        address:
            street: 123 East Main St.
            city: Anytown
            state: OR

Here's an example using a verbatim block for the address instead:

    person:
        name: Dan
        age:  37
        address {
            123 East Main St.
            Anytown, OR
        }
            
Finally, metavalues are applied by using a colon, which is sometimes a second colon in a given line.  Here's an example:

    thing:
        property: metavalue: value
        name: metavalue2 {
            blah blah blah
        }

This ninja defines an object named thing, which has two fields, both of which have metavalues.  Hopefully the names are self-explanatory.  Metavalues are used to provide additional context to fields, with the most common example being command fields like 'when', which is used to listen for events.  Our very first example used a 'ready' handler, which is basically the entry point of a standard ninja object.  Here's the line again:

        when: ready: print $data.what-to-say

Given what we now know, we can identify that when is the property name, ready is the metavalue, and the remainder of the line (some executable code) is the value of the property.  An object can have many (many) different fields named 'when', but they are all distinguished by their metavalues.  So you now know enough to read ninja pretty readily, but you're probably wondering about the 'print' thing.  Sometimes fields contain what appears to be (and is) executable code, what's the deal there?

## Imperative ninja

So far we've covered the declarative side of ninja