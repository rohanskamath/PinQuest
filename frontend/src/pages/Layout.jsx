import React from 'react'
import CustomNavigationBar from '../components/customComponents/CustomNavigationBar'
import { useSelector } from 'react-redux'
import { Outlet } from "react-router-dom";
import CustomFooter from '../components/customComponents/CustomFooter';

const Layout = () => {
    const placeName = useSelector((state) => state.location.placeName)
    return (
        <>
            <CustomNavigationBar placeName={placeName} />
            <main style={{ minHeight: "80vh" }}>
                <Outlet />
            </main>
            <CustomFooter />
        </>
    )
}

export default Layout
