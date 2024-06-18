# OnTheFlyCompany
## Objetivo Geral
Desenvolver uma aplicação que irá compartilhar diversas informações entre os “Módulos” que 
cada equipe será responsável.
#### Modulo: Companhia

## Objetivo Específico
Apresentar informações sobre um projeto que controlará as vendas de passagens áreas de um 
aeroporto denominado ON THE FLY. Os módulos são referentes aos controles de:
- Passageiros
- Companhias Aéreas
- Voos Agendados
- Venda de passagens
- OBS: Dúvidas poderão ser discutidas entre os grupos e, depois passadas e definidas com o 
representante do cliente (“Instrutor”). De acordo com as dúvidas esse documento poderá ser 
alterado.

## Cadastros Básicos
Todos os módulos deverão ter o CRUD completo, salvo exceções definidas em documento, 
valendo-se assim esta definição. Todos os campos de status servem para referenciar possíveis 
cadastros de restrição, salvo voos.
As operações de CRUD serão:
- Cadastrar (CREATE) (Todos os campos serão dados como necessários, salvo os que 
forem definidos como nullables). 
- Localizar (READ) - um registro específico a ser localizado, preferencialmente por seu 
dado que o definem como único.
- Editar (UPDATE) - Alterar os dados de um registro, desde que este não seja uma 
informação única.
- Apagar (DELETE) – Remove o registro, movendo-o a uma outra área do banco para 
que, se necessário, possa ser recuperado.

## Modulos

#### Entidade Passageiros
O aeroporto atende apenas pessoas físicas, sendo assim, não teremos que nos preocupar em 
vender para pessoas jurídicas. Os passageiros menores de 18 anos podem ser cadastrados,
mas não podem comprar nenhuma passagem. Os registros de restrição devem ter apenas as 
funções de incluir e remover o cadastro.

#### Entidade Companhias
Somente serão aceitos cadastros de pessoas jurídicas. Companhias aéreas podem ser 
cadastradas e devem conter pelo menos uma aeronave ativa no sistema. No caso de cadastros 
sem nome fantasia, o mesmo deve ser definido igual a razão social. Os registros de restrição 
devem ter apenas as funções de incluir e remover o cadastro.

#### Entidade Endereços
Deverá consultar a API do VIACEP, a partir do CEP, devendo ser informado apenas o número e 
complemento, caso haja. Em casos de CEP municipais, o campo de logradouro poderá ser 
inicialmente vazio.

#### Entidade Aeronaves
Deverão ser cadastradas informando um CNPJ de uma Companhia Aérea que deve estar ativa, 
para ser cadastrada. As aeronaves poderão ser cadastradas mesmo havendo a companhia 
cadastrada com restrição.

#### Entidade Voos
Deve conter os voos e passagens disponibilizados pelas companhias aéreas atendidas pelo 
aeroporto. Poderão ser operados, depois de cadastrado, somente para cancelamento do voo.
Os destinos possíveis deverão ser consultados por uma API, possibilitando somente voos 
nacionais. Nenhuma companhia poderá cadastrar voo quando tiver o cadastro de restrição 
positivo.

#### Entidade Vendas
Após o cadastro de voos, as passagens ficam disponíveis para serem vendidas de acordo com 
o máximo de assentos definido pela capacidade do avião. Na venda ou reserva não poderá 
constar o mesmo CPF na lista de passageiros do voo disponível. Nenhum passageiro na lista de 
vendas de passagem pode constar na lista de restritos. Caso aconteça, a venda não poderá ser 
registrada. O primeiro passageiro informado é deve ser sempre maior de 18 anos
