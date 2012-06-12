Serviços
=

Repositório
-

<span style="background-color:yellow">Descrição do padrão Repository</span>

Tendo em conta as caracteristicas do cliente RavenDB enunciadas optou-se por não criar uma camada de abstracção de acesso a dados. Para esta decisão contribuiu o facto de toda a infra-estrutura estar fortemente ligada ao RavenDB e a alteração deste SGBD para outro implicar alteração ao modelo de domínio.

Autorização
-

RavenDB Authorization Bundle

![Diagrama UML de autorização\label{autorizacao}](http://www.lucidchart.com/publicSegments/view/4fd773c2-23b4-4f9e-bea5-7a420adcb320/image.png)