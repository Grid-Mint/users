package main

import (
	"log"
	"net/http"

	"lacarte/users/api"
)

func main() {
	mux := http.NewServeMux()
	mux.HandleFunc("/", api.Handler)

	log.Println("listening on :8080")
	log.Fatal(http.ListenAndServe(":8080", mux))
}