# Usa una imagen base de Go para compilar la aplicación
FROM golang:1.20-alpine AS builder

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia los archivos de tu proyecto al directorio de trabajo
COPY . .

# Compila la aplicación
RUN go build -o app .

# Usa una imagen base ligera para ejecutar la aplicación compilada
FROM alpine:3.18

# Copia la aplicación compilada desde la fase de construcción
COPY --from=builder /app/app /app/app

# Expone el puerto en el que tu aplicación escuchará
EXPOSE 80

# Comando para ejecutar la aplicación
CMD ["/app/app"]
