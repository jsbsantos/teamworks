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

Na framework knockout a view é definida no HTML, utilizando o atributo [data-*](http://developers.whatwg.org/elements.html#embedding-custom-non-visible-data-with-the-data-*-attributes), o model é representado por um objecto javascript assim como o viewmodel. 



```RavenDB
quem disponibiliza 
como 
porque
```

Para alojamento da aplicação desenvolvida _________ duas opções appharbor(link) ou azure(link).
Depois de analizar as duas opções a opção recaiu sobre o appharbor pois suporta a base de dados de documentos utilizada neste projecto.


