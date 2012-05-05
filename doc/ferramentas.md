# Ferramentas usadas

Para a realização do projecto são usadas algumas aplicações externas, entre elas:

 * As frameworks .NET ASP.NET MVC e Web API;
 * As bibliotecas AutoMapper e AttributeRouting, ambas implementações .NET;
 * As frameworks javascript jQuery e Knockout;
 * A base de dados de documentos RavenDB;

Para instalar e gerir algumas destas aplicações é usada a extensão do Visual Studio NuGet. A extensão NuGet copia os ficheiros necessário e faz as alterações necessárias à solução do projecto de forma a ser possível a utilização de determinada aplicação.

```
asp.net mvc
	dto para validação e troca de informação
	automapper

web api
routing 
	traversal 
	attribute routing
```

A framework javascript **jQuery** disponibiliza uma forma de manipular o DOM e simplificar com o browser. A framework **Knockout** leva essa interação mais longe e através do padrão model-view-view model (MVVM) permitindo que as interações com o utilizador sejam mais simples e fluidas.

Neste padrão existem três interveniente, a view, o modelo e o view model. É através da view que o utilizador indica a ação a realizar, acção essa que é passada ao view-model. O view model envia e obtem dados do modelo e quando é alterado notifica os elementos da view que estão a observar os seus atributos.

<span style="background-color: yellow">IMAGEM</span>






Para a implementação do padrão a view é definida no HTML com a definição de metadados usando o atributo [data](http://developers.whatwg.org/elements.html#embedding-custom-non-visible-data-with-the-data-*-attributes) de HTML5 e o model e o viewmodel são objectos javascript. 

Ao definir no HTML atributos o utilizador liga o elemto DOM a um atributo do view model. Assim, qualquer interação com a view notifica o view model e alterações ao view model são observados pela view.

A custom data attribute is an attribute in no namespace whose name starts with the string "data-", has at least one character after the hyphen, is XML-compatible, and contains no characters in the range 



`explicar MVVM`

```RavenDB
quem disponibiliza 
como 
porque
```

Para alojamento da aplicação desenvolvida _________ duas opções appharbor(link) ou azure(link).
Depois de analizar as duas opções a opção recaiu sobre o appharbor pois suporta a base de dados de documentos utilizada neste projecto.


