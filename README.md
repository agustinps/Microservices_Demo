# Microservices Demo

## Proyecto con dos microservicios para mostrar ejemplo de comunicación

En primer lugar simulamos un microservicios de una tienda ms.shop con sus acciones de CrearPedido, BorrarPedido. 
Utilizamos el patrón CQRS, una base de datos SQLServer y aunque no está implementado así por no ser el objetivo del ejemplo, 
esto debería estar creado en un contenedor Docker.

En segundo lugar tenemos un microservicio almacén ms.storage, donde se dispone de las acciones PrepararPedido, EnviarPedido y VerificarStock.
En este caso se utiliza tambien una base de datos SQLServer por simplicidad, pero podría ser otra base de datos cualquiera y lógicamente debería estar
igualmente en un contenedor Docker.

Para la comunicación tenemos un proyecto compartido ms.communications.rabbitmq, donde se implementa la cola de mensajería
RabbitMQ: 

1) La acción "CrearPedido" se comunica con el microservicio almacén mediante http utilizando Redfit para verificar si hay stock con la acción "StockDisponible" :

	a) Si no hay stock emite una notificación mediante el patrón CQRS avisando al cliente de que su pedido no puede ser enviado. 
	
	b) Si hay stock el productor "CrearPedido" envía un mensaje a la cola "OrderCreated" a la que está suscrito el consumidor "ProductConsumer".

2) Este consumidor "ProductConsumer" una vez termina de preparar el pedido (en sentido figurado):

	a) Envía un mensaje a la cola "OrderPrepared", a la que está suscrito el consumidor "OrderConsumer" que enviará un correo al cliente indicando que su pedido ya está listo para enviarse. 
	
	b) También se envía un mensaje a "OrderShipped" para avisar de que se ha enviado el pedido

3) El consumidor "OrderConsumer" leer este mensaje y envía un correo al cliente avisando de que su pedido se ha enviado.

Como podemos ver, tenemos notificaciones internas con CQRS, comunicación con mnesajería de colas con RabbitMQ y comunicación por http con Refit.