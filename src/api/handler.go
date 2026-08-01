package api

import (
	"log"
	"net/http"
)

func ListUsers(w http.ResponseWriter, r *http.Request) {
	log.Printf("%s %s", r.Method, r.URL.Path)
	w.WriteHeader(http.StatusOK)
	w.Write([]byte("users"))
}

func CreateUser(w http.ResponseWriter, r *http.Request) {
	log.Printf("%s %s", r.Method, r.URL.Path)
	w.WriteHeader(http.StatusOK)
	w.Write([]byte("user created"))
}

func GetUser(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")
	log.Printf("%s %s id=%s", r.Method, r.URL.Path, id)
	w.WriteHeader(http.StatusOK)
	w.Write([]byte("user " + id))
}
