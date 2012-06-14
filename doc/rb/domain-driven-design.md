Domain Driven Design
=

\label{app:ddd}

Esta abordagem ao desenvolvimento de software defende que o foco principal deve ser o modelo e lógica do domínio do problema. 

O modelo de domínio é o conjunto de vários termos, diagramas e conceitos que representam a informação e o comportamento da aplicação face ao problema. Para representar o modelo de domínio podem ser usados diagramas UML, texto detalhado, esquemas entidade-associação, use cases, etc. Um aspecto importante é compreensão do modelo de domínio por todos os intervenientes no projecto (e.g. arquitectos de software, programadores, cliente). Aos elementos do modelo de domínio dá-se o nome de objectos de domínio.

Entidades, *value objects*, agregados, fábricas e repositórios são alguns conceitos transversais a qualquer modelo de domínio. Uma entidade representa um objecto do modelo que tem um identificador único em todo o seu tempo de vida na aplicação e pode ser acedido através desse identificador.

Um *value object*, assim como uma entidade, é representado pelas suas características e atributos mas não tem identidade no sistema, ou seja *value objects* com os mesmas características e atributos são considerados o mesmo.

No modelo de domínio quando um grupo de objectos é tratado como uma unidade, no que diz respeito á informação que estes representam, são considerados agregados. Um agregado define um limite e tem como raiz uma entidade. É através dessa entidade que os outros elementos do agregado são acedidos.

As fábricas e os repositórios são usados para gerir o tempo de vida das entidades. As fábricas são usadas na criação e por vezes para abstrair o tipo concreto do objecto criado. Os repositórios são mecanismos que encapsulam a obtenção, alteração e a procura dos dados do sistema.