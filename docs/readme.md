# docs

This directory contains the documentation and diagrams I used.

I started with the sketches in [0 initial walk through](0%20initial%20walk%20through) to get my thoughts down on paper.

![0 initial walk through/initial walk through thoughts.png](0%20initial%20walk%20through/initial%20walk%20through%20thoughts.png)

there are some additional thoughts in [0 initial walk through/initial walk through thoughts.md](0%20initial%20walk%20through/initial%20walk%20through%20thoughts.md) that I wrote down while thinking through the problem.

Then I turned those thoughts into some entities and class diagrams just to wrap my head around what the major components would be.
![1 initial diagrams/initial](1%20initial%20diagrams/initial%20entities.png)
![1 initial diagrams/initial scenarios](1%20initial%20diagrams/initial%20scenarios.excalidraw.png)

Then I took my took those sketches and produced the mermaid diagrams and hashed some rough requirements and implementation notes. (these should be viewable in github, locally you may need a mermaid plugin.) I did use AI tooling to help me get to these diagrams, but it was a very hands-on process, and the thought process and layout, and what diagrams to use was all mine

- [1 initial diagrams/class-diagram.md](1%20initial%20diagrams/class-diagram.md)
- [1 initial diagrams/entity-relationship-diagram.md](1%20initial%20diagrams/entity-relationship-diagram.md).

**note** I did not go back and update these as I went, normally I would, so they may not match the implementation exactly but are roughly what I used to guide the design.

In the end I wound up implementing a much simpler sale without an inventory component or flow, but I think the design I illustrate here would work better for a real system (think of a bicycle version of a B&H photography store if you've every been to the big one in new york city).
