Domain Driven Design
=

\label{app:domaindrivendesign}

Esta abordagem defende que o foco principal no desenvolvimento de software é o modelo de domínio do problema.

O modelo de domínio é o conjunto de vários termos, diagramas e conceitos que representam a informação e o comportamento da aplicação face ao problema.
Para representar o modelo de domínio podem ser usados diagramas UML, texto detalhado, esquemas entidade-associação, use cases, etc.
Um aspecto importante é compreensão do modelo de domínio por todos os intervenientes no projecto (e.g. arquitectos de software, programadores, cliente).
Aos elementos do modelo de domínio dá-se o nome de objectos de domínio.

Alguns objectos de domínio transversais a qualquer projecto são as entidades, os *value objects*, os agregados, as fábricas e os repositórios.

Uma entidade representa um objecto do modelo possível de identificar únivocamente em todo o seu tempo de vida na aplicação.

Um *value object*, assim como uma entidade, é representado pelas suas características e atributos mas não tem identidade no sistema, ou seja *value objects* com os mesmas características e atributos são considerados iguais.

No modelo de domínio quando um grupo de objectos é tratado como uma unidade, no que diz respeito á informação que estes representam, são considerados agregados. Um agregado define um limite e tem como raiz uma entidade. É através dessa entidade que os outros elementos do agregado são acedidos.

As fábricas e os repositórios são usados para gerir o tempo de vida das entidades. As fábricas são usadas na criação e por vezes para abstrair o tipo concreto do objecto criado. Os repositórios são mecanismos que encapsulam a obtenção, alteração e persistência dos dados no sistema.